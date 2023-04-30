using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /*Editor settings.*/
    [SerializeField] private Transform BlocksParent = null;
    
    /*0 = start block. 1 = normal block. 2 = end block.*/
    [SerializeField] private List<GameObject> Blocks = null;

    /*0 = empty plot. 1 = small house plot. 2 = large house plot.*/
    [SerializeField] private List<GameObject> Plots = null;

    /*Defines what a set of blocks must have.*/
    [SerializeField] private List<Phase> PhasesList = null;

    [System.Serializable] public struct Phase
    {

        public int MaxPlots;
        public int MaxPlotDropOffZones;
        public int AmountOfPackagesToDeliver;
        public int PhaseEnd;
    }

    /*Private values.*/
    private Vector3 BlockSpawnPoint = Vector3.zero;
    private Transform PreviousBlock = null;
    private Transform CurrentBlock = null;
    private int CurrentPhase = 0;

    private enum LevelState
    {
        Playing,
        Done,
        Transitioning
    }
    private LevelState CurrentLevelState = LevelState.Playing;

    private PackageVerifyer Verifyer = null;
    private ScoreManager SM = null;

    // Start is called before the first frame update
    void Start()
    {
        Verifyer = FindObjectOfType<PackageVerifyer>();
        SM = FindObjectOfType<ScoreManager>();
        Phase Empty = new Phase();
        SpawnBlock(0, Empty);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLevelState == LevelState.Transitioning)
            WaitForPlayer();

        /*Check if Block is finished or ran out of time.*/
        if (CurrentLevelState == LevelState.Playing)
            IsBlockDone();

        /*Generate new block if level state is done.*/
        if (CurrentLevelState == LevelState.Done)
            GenerateNewBlock();
    }

    public void ForceTriggerNextBlock()
    {
        CurrentLevelState = LevelState.Done;
        CurrentPhase++;
    }

    private void IsBlockDone()
    {
        BlockControll TempBlockControll = CurrentBlock.GetComponent<BlockControll>();
        bool Done = TempBlockControll.IsBlockReady();

        if (Done)
        {
            SM.AddToTotalScore(100);
            CurrentLevelState = LevelState.Done;
            CurrentPhase++;
        }
    }

    private void GenerateNewBlock()
    {
        int BlockIndex = 1;

        Phase TempPhase = GetCurrentPhase();

        if (CurrentPhase == 0)
            BlockIndex = 0;
        else if (PhasesList[PhasesList.Count - 1].PhaseEnd < CurrentPhase)
            BlockIndex = 2;

        SpawnBlock(BlockIndex, TempPhase);
    }

    private Phase GetCurrentPhase()
    {
        for (int i = 0; i < PhasesList.Count; i++)
        {
            if (PhasesList[i].PhaseEnd >= CurrentPhase)
                return (PhasesList[i]);
        }

        return (new Phase());
    }

    private void SpawnBlock(int BlockIndex, Phase PhaseData)
    {
        GameObject TempBlock = Instantiate<GameObject>(Blocks[BlockIndex], BlockSpawnPoint, transform.rotation, BlocksParent);

        BlockSpawnPoint += new Vector3(0f, 0f, 40f);

        PreviousBlock = CurrentBlock;
        CurrentBlock = TempBlock.transform;

        if (BlockIndex != 1)
        {
            CurrentLevelState = LevelState.Transitioning;
            return;
        }

        /*Spawn Plots.*/
        SpawnPlots(PhaseData, TempBlock);
    }

    private void SpawnPlots(Phase PhaseData, GameObject TempBlock)
    {
        BlockControll TempBlockControll = TempBlock.GetComponent<BlockControll>();

        int BuildingsSpawned = 0;

        for (int i = 0; i < 4; i++)
        {
            int SpawnIndex = 0;

            if (BuildingsSpawned < PhaseData.MaxPlots)
            {
                int RandomValue = Random.Range(PhaseData.MaxPlots + i, 4 + 1);
                bool SpawnBuilding = (1f <= (float)(RandomValue / 4));

                if (!SpawnBuilding)
                    SpawnIndex = 0;
                else
                {
                    SpawnIndex = 1;
                    BuildingsSpawned++;
                }
            }

            Transform PlotParent = TempBlockControll.GetPlotParent(i);
            Vector3 SpawnPoint = PlotParent.position;
            Quaternion SpawnRotation = new Quaternion(0, (i >= 2 ? 180 : 0), 0, 0);

            GameObject TempPlot = Instantiate<GameObject>(Plots[SpawnIndex], SpawnPoint, SpawnRotation, PlotParent);

            if (SpawnIndex == 0)
                continue;

            /*Send random string list of package names and max active drop off zones.*/
            PlotControll TempPlotControll = TempPlot.GetComponent<PlotControll>();

            List<string> TempPackages = new List<string>();
            for (int a = 0; a < PhaseData.AmountOfPackagesToDeliver; a++)
            {
                string RandomPackage = Verifyer.GetRandomPackageName();
                TempPackages.Add(RandomPackage);
            }

            TempPlotControll.SetData(TempPackages, PhaseData.MaxPlotDropOffZones);
        }
        CurrentLevelState = LevelState.Transitioning;
    }

    private void WaitForPlayer()
    {
        if (CurrentPhase == 0)
        {
            CurrentLevelState = LevelState.Playing;
            return;
        }

        BlockControll TempCurrentBlockControll = CurrentBlock.GetComponent<BlockControll>();
        BlockControll TempPreviousBlockControll = PreviousBlock.GetComponent<BlockControll>();

        TempCurrentBlockControll.InvisibleStartWallActive(false);
        TempPreviousBlockControll.InvisibleEndWallActive(false);

        if (!IsPlayerInsideArea(TempCurrentBlockControll))
            return;

        TempCurrentBlockControll.InvisibleStartWallActive(true);

        Destroy(PreviousBlock.gameObject);
        PreviousBlock = null;
        CurrentLevelState = LevelState.Playing;
    }

    private bool IsPlayerInsideArea(BlockControll Block)
    {
        bool Result = Block.IsPlayerInside();
        return (Result);
    }
}
