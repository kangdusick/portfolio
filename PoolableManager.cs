//ToolPoolableManagerGen 의해 자동으로 생성된 스크립트입니다..
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using UnityEngine;
using Component = UnityEngine.Component;
using UnityEngine.AddressableAssets;
public enum EPrefab
{
    [Description("Blood_splash_BW_01")]
	Blood_splash_BW_01 = -1343573103,
	[Description("health_up")]
	health_up = -143662253,
	[Description("StunEffect")]
	StunEffect = -2340624,
	[Description("Smoke_2")]
	Smoke_2 = -926822263,
	[Description("Explosion_green")]
	Explosion_green = 285503248,
	[Description("Blood_splash_BW_10")]
	Blood_splash_BW_10 = -1343573105,
	[Description("Lazer_green")]
	Lazer_green = 1901246201,
	[Description("Destruction_air_normal")]
	Destruction_air_normal = -979573654,
	[Description("fire_small")]
	fire_small = 1340392563,
	[Description("Blood_splash_BW_07")]
	Blood_splash_BW_07 = -1343573097,
	[Description("Smoke_1")]
	Smoke_1 = -926822262,
	[Description("Blood_splash_BW_03")]
	Blood_splash_BW_03 = -1343573101,
	[Description("expl_02_04")]
	expl_02_04 = -183921224,
	[Description("Blood_splash_BW_09")]
	Blood_splash_BW_09 = -1343573095,
	[Description("Blood_splash_BW_05")]
	Blood_splash_BW_05 = -1343573099,
	[Description("Blood_splash_BW_06")]
	Blood_splash_BW_06 = -1343573098,
	[Description("Blood_splash_BW_04")]
	Blood_splash_BW_04 = -1343573100,
	[Description("smoke_3")]
	smoke_3 = -934557720,
	[Description("Trail_green")]
	Trail_green = -60743203,
	[Description("Blood_splash_BW_08")]
	Blood_splash_BW_08 = -1343573096,
	[Description("Mine_normal")]
	Mine_normal = 1335929300,
	[Description("Flame_Center_3")]
	Flame_Center_3 = 776081462,
	[Description("WorldMapItem")]
	WorldMapItem = -140379028,
	[Description("CharacterCard_Mini")]
	CharacterCard_Mini = 207553664,
	[Description("Moving_Area")]
	Moving_Area = -242377733,
	[Description("ShopLifterOrc")]
	ShopLifterOrc = 1633966965,
	[Description("ClubOgre")]
	ClubOgre = 672386274,
	[Description("ShotPutOgre")]
	ShotPutOgre = -86094805,
	[Description("IronOrc")]
	IronOrc = 113371133,
	[Description("Boar")]
	Boar = 4594587,
	[Description("Rat")]
	Rat = 132536,
	[Description("BackOgre")]
	BackOgre = -464481877,
	[Description("Crawler")]
	Crawler = 582587161,
	[Description("MageOrc")]
	MageOrc = -125825371,
	[Description("BabyOrc")]
	BabyOrc = -844800001,
	[Description("BungeeOrc")]
	BungeeOrc = 846792011,
	[Description("PumpkinHeadOrc")]
	PumpkinHeadOrc = -1545174125,
	[Description("Ogre")]
	Ogre = 4500634,
	[Description("priestOrc")]
	priestOrc = 1579903020,
	[Description("WolfOrc")]
	WolfOrc = -1609726053,
	[Description("NecromancerOrc")]
	NecromancerOrc = -1472681130,
	[Description("ArcherOrc")]
	ArcherOrc = -2027336856,
	[Description("SuperManOrc")]
	SuperManOrc = -892030810,
	[Description("MindEyeOrc")]
	MindEyeOrc = -50252404,
	[Description("ThiefOrc")]
	ThiefOrc = 392515109,
	[Description("AdultOrc")]
	AdultOrc = -349010283,
	[Description("HydeOrc")]
	HydeOrc = -1530061445,
	[Description("NinjaOrc")]
	NinjaOrc = 1126833861,
	[Description("ThiefOrcCoin")]
	ThiefOrcCoin = 704169438,
	[Description("ChemicalOrc")]
	ChemicalOrc = 1209885551,
	[Description("Ram")]
	Ram = 132513,
	[Description("Ox")]
	Ox = 4690,
	[Description("Sheep")]
	Sheep = 128549172,
	[Description("Hound")]
	Hound = 136373279,
	[Description("LittleBunny")]
	LittleBunny = -355285743,
	[Description("Bull")]
	Bull = 4596776,
	[Description("Bunny")]
	Bunny = 142506221,
	[Description("Butcher")]
	Butcher = -518556548,
	[Description("Egg")]
	Egg = 146874,
	[Description("Pug")]
	Pug = 133945,
	[Description("Dog")]
	Dog = 154227,
	[Description("Chicken")]
	Chicken = 300593944,
	[Description("SheepFur")]
	SheepFur = -1502507921,
	[Description("BlackChicken")]
	BlackChicken = -362185087,
	[Description("Puggy")]
	Puggy = 128718073,
	[Description("Chick")]
	Chick = 143329393,
	[Description("RamFur")]
	RamFur = -347342372,
	[Description("Feeder")]
	Feeder = 229145658,
	[Description("Bullet")]
	Bullet = 122533527,
	[Description("Grenade")]
	Grenade = -158452453,
	[Description("ToastMessage")]
	ToastMessage = -308916051,
	[Description("Kunai")]
	Kunai = 136269573,
	[Description("DamageText")]
	DamageText = 92199005,
	[Description("CannonBall")]
	CannonBall = -1092394365,
	[Description("Arrow")]
	Arrow = 138096490,
	[Description("Orge_Stone")]
	Orge_Stone = -1926868338,
	[Description("PopPause")]
	PopPause = 1716639702,
	[Description("PopTimeReward")]
	PopTimeReward = 1867927842,
	[Description("PopSetting")]
	PopSetting = 547262344,
	[Description("PopShop")]
	PopShop = -83011372,
	[Description("PopInGameSetting")]
	PopInGameSetting = 1942056165,
	[Description("PopWorldMap")]
	PopWorldMap = -1785377970,
	[Description("Meat")]
	Meat = 4316862,
	[Description("ButterFlyCoin")]
	ButterFlyCoin = 2008441843,
	[Description("CircleRangeIndicator")]
	CircleRangeIndicator = 826863821,
	[Description("Prey")]
	Prey = 4157339,
	[Description("Scarecrow")]
	Scarecrow = 968922356,
	[Description("BombScarecrow")]
	BombScarecrow = 815397108,
	[Description("Horse")]
	Horse = 136369254,
	[Description("Pony")]
	Pony = 4177443,
	[Description("BossMonkey")]
	BossMonkey = 1827540795,
	[Description("Monkey")]
	Monkey = -156052680,
	[Description("Health_Up_green")]
	Health_Up_green = 574236407,
	[Description("Banana")]
	Banana = 110914336,
	[Description("Goat")]
	Goat = 4747386,
	[Description("WildGoat")]
	WildGoat = -1005585874,
	[Description("MohawkBat")]
	MohawkBat = 859570425,
	[Description("Bat")]
	Bat = 147880,
	[Description("Farmer")]
	Farmer = 224290938,
	[Description("PopSelectAbility")]
	PopSelectAbility = -484372894,
	[Description("Carpenter")]
	Carpenter = -851833495,
	[Description("summer")]
	summer = 639775528,
	[Description("fall")]
	fall = 5719224,
	[Description("spring")]
	spring = 644339704,
	[Description("Tornado")]
	Tornado = -600822346,
	[Description("Thunder")]
	Thunder = -814478541,
	[Description("Thunder_Ground")]
	Thunder_Ground = -713275757,
	[Description("Flood")]
	Flood = 146157617,
	[Description("Panther")]
	Panther = -282960231,
	[Description("PantherClaw")]
	PantherClaw = -1018465352,
	[Description("WhitePanther")]
	WhitePanther = -1601603334,
	[Description("GoldChicken")]
	GoldChicken = 1877588074,
	[Description("Turkey")]
	Turkey = -183129023,
	[Description("Darkness")]
	Darkness = 2404242,
	[Description("Cage")]
	Cage = 4622631,
	[Description("ChickMeat")]
	ChickMeat = 1108744008,
	[Description("MoneyText")]
	MoneyText = -1663436372,
	[Description("Yeti")]
	Yeti = 3911032,
	[Description("BlackEgg")]
	BlackEgg = 2018065903,
	[Description("GoldEgg")]
	GoldEgg = -294741188,
	[Description("Golem")]
	Golem = 147179217,
	[Description("Ground_Hit_1_green")]
	Ground_Hit_1_green = -1651440560,
	[Description("LandingSmoke")]
	LandingSmoke = 1927483223,
	[Description("OrcMeat")]
	OrcMeat = 1332860794,
	[Description("Auriel")]
	Auriel = -9304147,
	[Description("PopGameLoseResult")]
	PopGameLoseResult = 676658582,
	[Description("PopGameWinResult")]
	PopGameWinResult = 85530127,
	[Description("BlackPrey")]
	BlackPrey = -1864062880,
	[Description("HungryIcon")]
	HungryIcon = 1516999255,
	[Description("MotherChicken")]
	MotherChicken = -810211761,
	[Description("Muzzle_dron_normal")]
	Muzzle_dron_normal = -452667172,
	[Description("Muzzle_shotgun_normal")]
	Muzzle_shotgun_normal = 655240863,
	[Description("Ghost")]
	Ghost = 147022072,
	[Description("PantherGem")]
	PantherGem = 1352614166,
	[Description("Gem")]
	Gem = 153076,
	[Description("Grain")]
	Grain = 147317236,
	[Description("Milk")]
	Milk = 4312834,
	[Description("Coin")]
	Coin = 4620780,
	[Description("ChickenMeat")]
	ChickenMeat = -393504407,
	[Description("PantherPelt")]
	PantherPelt = -1017821188,
	[Description("WhitePantherPelt")]
	WhitePantherPelt = -591711657,
	[Description("CharacterCardSlot")]
	CharacterCardSlot = 145655302,
	[Description("PopChecBuyAgain")]
	PopChecBuyAgain = 764979893,
	[Description("Rock")]
	Rock = 4117968,
	[Description("explosion_3")]
	explosion_3 = -1906694434,
	[Description("SilverOre")]
	SilverOre = -26817016,
	[Description("PlatinumOre")]
	PlatinumOre = 517589465,
	[Description("IronOre")]
	IronOre = 113371131,
	[Description("GoldOre")]
	GoldOre = -294754233,
	[Description("DiaOre")]
	DiaOre = 294065597,
	[Description("CopperOre")]
	CopperOre = -1824520676,
	[Description("FireBall")]
	FireBall = 283311432,
	[Description("winter")]
	winter = 780740436,
	[Description("Teleport_blue")]
	Teleport_blue = 466208961,
	[Description("PopCheckReallyGoOut")]
	PopCheckReallyGoOut = 1655938981,
	[Description("Wasp")]
	Wasp = 4265522,
	[Description("PopPatchNote")]
	PopPatchNote = -1760077578,
	[Description("FarmRat")]
	FarmRat = -1636955420,
	[Description("MeatBug")]
	MeatBug = -245323892,
	[Description("OrcTotem")]
	OrcTotem = -1654746870,
	[Description("Blizzard")]
	Blizzard = -2024602791,
	[Description("PopSelectedAblityList")]
	PopSelectedAblityList = -50015606,
	[Description("AbilityListIcon")]
	AbilityListIcon = -1027665664,
	[Description("Log")]
	Log = 146043,
	[Description("GoldLog")]
	GoldLog = -294749955,
	[Description("BomberMan")]
	BomberMan = 226441830,
	[Description("Donkey")]
	Donkey = 299790931,
	[Description("Burro")]
	Burro = 142478459,
	[Description("BattleGolem")]
	BattleGolem = -1248138629,
	[Description("GoldenSheep")]
	GoldenSheep = -1382942019,
	[Description("GoldenSheepFur")]
	GoldenSheepFur = -1899447744,
	[Description("GoldenOgre")]
	GoldenOgre = -736749391,
	[Description("MoneyParticle")]
	MoneyParticle = -1239494837,
	[Description("EliteOrc")]
	EliteOrc = -1602480226,
	[Description("Uzi_red")]
	Uzi_red = 1111572467,
	[Description("PopDeleteAccount")]
	PopDeleteAccount = -1585822618,
	[Description("BundleOrigin")]
	BundleOrigin = 566228889,
	[Description("BountyChest")]
	BountyChest = 1560798221,
	[Description("PoisonRain")]
	PoisonRain = -29584207,
	[Description("BlackOre")]
	BlackOre = 2018071260,
	[Description("Amazon")]
	Amazon = -31945125,
	[Description("AmazonSpear")]
	AmazonSpear = 11189622,
	[Description("RedAdultOrc")]
	RedAdultOrc = -487511574,
	[Description("RedOgre")]
	RedOgre = 2016585321,
	[Description("RedBabyOrc")]
	RedBabyOrc = 1193107942,
	[Description("volcano")]
	volcano = 1905952747,
	[Description("VolcanoWorm")]
	VolcanoWorm = 1909199950,
	[Description("Meteor")]
	Meteor = -147117525,
	[Description("Explosion_normal")]
	Explosion_normal = 2300838,
	[Description("WallLava")]
	WallLava = 18654957,
	[Description("LavaBomb")]
	LavaBomb = -1237678715,
	[Description("HpBar")]
	HpBar = 137188566,
	[Description("Asteroid")]
	Asteroid = -1664524808,
	[Description("PopCharacterCardInfo")]
	PopCharacterCardInfo = 1748105987,
	[Description("PopCommon")]
	PopCommon = 1367575927,
	[Description("Skeleton")]
	Skeleton = -503504990,
	[Description("KingStone")]
	KingStone = -2015187395,
	[Description("Bundle")]
	Bundle = 122716015,
	[Description("BountyChestOrigin")]
	BountyChestOrigin = 2002844063,
	[Description("PopFarmerPetSetting")]
	PopFarmerPetSetting = 562785628,
	[Description("PopSkillTraining")]
	PopSkillTraining = -142612379,
	[Description("FireMageExplosion")]
	FireMageExplosion = -1611888586,
	[Description("PopIngameShop")]
	PopIngameShop = -1398235561,
	[Description("InGameShopItem")]
	InGameShopItem = 893300025,
	[Description("PopInGameEvent")]
	PopInGameEvent = 682232909,
	[Description("SplashMeleeAttack")]
	SplashMeleeAttack = -1229169834,
	[Description("chaos")]
	chaos = 174843917,
	[Description("Blood_splash_BW_02")]
	Blood_splash_BW_02 = -1343573102,
	
}



public class AddEPrefab : MonoBehaviour
{
    public EPrefab eprefab; // EPrefab 타입의 프리팹
    public bool isPoolable; // 풀링 가능 여부
    public ManagedAction OnDestroy = new(); // 객체 파괴 시 실행되는 액션
    public Coroutine destroyCoroutine; // 파괴 코루틴
}

public class PoolableManager : MonoBehaviour
{
    private Dictionary<EPrefab, GameObject> _originPrefabs = new(); // 원본 프리팹 딕셔너리
    public Dictionary<GameObject, AddEPrefab> goEprefabFinder = new(); // GameObject와 AddEPrefab 매핑
    private Dictionary<EPrefab, Stack<AddEPrefab>> _poolableObjects = new(); // 풀링 가능한 객체 스택
    public Dictionary<EPrefab, Transform> _poolableParent = new(); // 풀링 가능한 객체의 부모 트랜스폼
    private HashSet<EPrefab> _preloadedPrefabs = new HashSet<EPrefab>(); // 미리 로드된 프리팹 집합
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(); // 취소 토큰 소스
    private static PoolableManager _instance; // 싱글톤 인스턴스
    public static PoolableManager Instance
    {
        get
        {
            if (ReferenceEquals(_instance, null))
            {
                GameObject go = new GameObject("PoolableManager");
                _instance = go.AddComponent<PoolableManager>();
            }
            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }
    private Dictionary<EPrefab, UniTaskCompletionSource<GameObject>> _loadingTasks = new Dictionary<EPrefab, UniTaskCompletionSource<GameObject>>(); // 로딩 작업 딕셔너리

    public void PreLoadAsset(EPrefab ePrefab)
    {
        if (!_preloadedPrefabs.Contains(ePrefab))
        {
            _preloadedPrefabs.Add(ePrefab);
            LoadAssetAsync(ePrefab);
        }
    }
    
    // 동기식으로 자산을 로드하는 메서드
    private GameObject LoadAsset(EPrefab ePrefab)
    {
        if (!_originPrefabs.ContainsKey(ePrefab))
        {
            var go2 = Addressables.LoadAssetAsync<GameObject>(ePrefab.OriginName()).WaitForCompletion();
            _originPrefabs[ePrefab] = go2;
        }
        return _originPrefabs[ePrefab];
    }
    
    // 비동기식으로 자산을 로드하는 메서드
    private async UniTask<GameObject> LoadAssetAsync(EPrefab ePrefab)
    {
        if (!_originPrefabs.ContainsKey(ePrefab))
        {
            if (!_loadingTasks.TryGetValue(ePrefab, out var completionSource))
            {
                completionSource = new UniTaskCompletionSource<GameObject>();
                _loadingTasks[ePrefab] = completionSource;

                try
                {
                    var go2 = await Addressables.LoadAssetAsync<GameObject>(ePrefab.OriginName()).ToUniTask(cancellationToken: _cancellationTokenSource.Token);
                    _originPrefabs[ePrefab] = go2;
                    completionSource.TrySetResult(go2);
                }
                catch (System.Exception e)
                {
                    completionSource.TrySetException(e);
                    Debug.Log("Loading of " + ePrefab.OriginName() + " was cancelled.");
                }
                finally
                {
                    _loadingTasks.Remove(ePrefab);
                }
            }

            try
            {
                return await completionSource.Task;
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return _originPrefabs[ePrefab];
        }
    }
    
    // 프리팹을 인스턴스화하는 메서드들
    public GameObject Instantiate(string ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null)
    {
        return Instantiate(ePrefab.ParseEnum<EPrefab>(), position, localScale, rotation, parentTransform);
    }

    public GameObject Instantiate(EPrefab ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null)
    {
        LoadAsset(ePrefab);
        GameObject go = GetOrCreateGameObject(ePrefab);
        SetupGameObject(go, position, rotation, localScale, parentTransform);
        return go;
    }

    public T Instantiate<T>(string ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null) where T : Component
    {
        return Instantiate<T>(ePrefab.ParseEnum<EPrefab>(), position, localScale, rotation, parentTransform);
    }

    public T Instantiate<T>(EPrefab ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null) where T : Component
    {
        LoadAsset(ePrefab);
        GameObject go = GetOrCreateGameObject(ePrefab);
        SetupGameObject(go, position, rotation, localScale, parentTransform);
        return go.GetCashComponent<T>();
    }

    public async UniTask<GameObject> InstantiateAsync(string ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null)
    {
        return await InstantiateAsync(ePrefab.ParseEnum<EPrefab>(), position, localScale, rotation, parentTransform);
    }

    public async UniTask<GameObject> InstantiateAsync(EPrefab ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null)
    {
        await LoadAssetAsync(ePrefab);

        GameObject go = await GetOrCreateGameObjectAsync(ePrefab);
        await UniTask.SwitchToMainThread();
        SetupGameObject(go, position, rotation, localScale, parentTransform);

        return go;
    }

    public async UniTask<T> InstantiateAsync<T>(string ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null) where T : Component
    {
        return await InstantiateAsync<T>(ePrefab.ParseEnum<EPrefab>(), position, localScale, rotation, parentTransform);
    }

    public async UniTask<T> InstantiateAsync<T>(EPrefab ePrefab, Vector3 position = default, Vector3 localScale = default, Quaternion rotation = default, Transform parentTransform = null) where T : Component
    {
        await LoadAssetAsync(ePrefab);

        GameObject go = await GetOrCreateGameObjectAsync(ePrefab);
        await UniTask.SwitchToMainThread();
        SetupGameObject(go, position, rotation, localScale, parentTransform);

        return go.GetCashComponent<T>();
    }
   
    // 게임 오브젝트 설정 메서드
    private void SetupGameObject(GameObject go, Vector3 position, Quaternion rotation, Vector3 localScale, Transform parentTransform)
    {
        if (parentTransform != null && go.transform.parent != parentTransform)
        {
            go.transform.SetParent(parentTransform, worldPositionStays: false);
        }

        go.transform.position = position;
        go.transform.rotation = rotation;

        if (localScale != default(Vector3))
        {
            go.transform.localScale = localScale;
        }
    }
    
    // 비동기식으로 게임 오브젝트를 생성하거나 가져오는 메서드
    private async UniTask<GameObject> GetOrCreateGameObjectAsync(EPrefab ePrefab)
    {
        if (!_poolableParent.ContainsKey(ePrefab))
        {
            CreateNewPool(ePrefab);
        }

        if (_poolableObjects[ePrefab].Count > 0)
        {
            AddEPrefab addEprefab = null;
            do
            {
                addEprefab = _poolableObjects[ePrefab].Pop();
                if (addEprefab.isPoolable)
                {
                    addEprefab.isPoolable = false;
                    addEprefab.gameObject.SetActive(true);

                    if (addEprefab.destroyCoroutine != null)
                    {
                        StopCoroutine(addEprefab.destroyCoroutine);
                        addEprefab.destroyCoroutine = null;
                    }
                    addEprefab.OnDestroy = new();
                    return addEprefab.gameObject;
                }
            } while (_poolableObjects[ePrefab].Count > 0);

            return await CreateNewGameObjectAsync(ePrefab);
        }
        else
        {
            return await CreateNewGameObjectAsync(ePrefab);
        }
    }
    
    // 동기식으로 게임 오브젝트를 생성하거나 가져오는 메서드
    private GameObject GetOrCreateGameObject(EPrefab ePrefab)
    {
        if (!_poolableParent.ContainsKey(ePrefab))
        {
            CreateNewPool(ePrefab);
        }

        if (_poolableObjects[ePrefab].Count > 0)
        {
            AddEPrefab addEprefab = null;
            do
            {
                addEprefab = _poolableObjects[ePrefab].Pop();
                if (addEprefab.isPoolable)
                {
                    addEprefab.isPoolable = false;
                    addEprefab.gameObject.SetActive(true);

                    if (addEprefab.destroyCoroutine != null)
                    {
                        StopCoroutine(addEprefab.destroyCoroutine);
                        addEprefab.destroyCoroutine = null;
                    }
                    addEprefab.OnDestroy = new();
                    return addEprefab.gameObject;
                }
            } while (_poolableObjects[ePrefab].Count > 0);

            return CreateNewGameObject(ePrefab);
        }
        else
        {
            return CreateNewGameObject(ePrefab);
        }
    }
    
    // 새로운 풀을 생성하는 메서드
    private void CreateNewPool(EPrefab ePrefab)
    {
        var newQueue = new Stack<AddEPrefab>();
        _poolableObjects.Add(ePrefab, newQueue);

        var parent = new GameObject($"{ePrefab}Parent");
        parent.transform.SetParent(transform);
        _poolableParent.Add(ePrefab, parent.transform);
    }
    
    // 비동기식으로 새로운 게임 오브젝트를 생성하는 메서드
    private async UniTask<GameObject> CreateNewGameObjectAsync(EPrefab ePrefab)
    {
        var newGameObjectOperation = UnityEngine.Object.InstantiateAsync(_originPrefabs[ePrefab], _poolableParent[ePrefab].transform);
        await UniTask.WaitUntil(() => newGameObjectOperation.isDone);
        var addEPrefab = newGameObjectOperation.Result[0].AddComponent<AddEPrefab>();
        addEPrefab.eprefab = ePrefab;
        addEPrefab.isPoolable = false;
        goEprefabFinder.Add(newGameObjectOperation.Result[0], addEPrefab);

        return newGameObjectOperation.Result[0];
    }
    
    // 동기식으로 새로운 게임 오브젝트를 생성하는 메서드
    private GameObject CreateNewGameObject(EPrefab ePrefab)
    {
        var newGameObject = Instantiate(_originPrefabs[ePrefab], _poolableParent[ePrefab].transform, true);
        var addEPrefab = newGameObject.AddComponent<AddEPrefab>();
        addEPrefab.eprefab = ePrefab;
        addEPrefab.isPoolable = false;
        goEprefabFinder.Add(newGameObject, addEPrefab);

        return newGameObject;
    }
    
    // 게임 오브젝트를 파괴하는 메서드
    public void Destroy(GameObject gameObj, float delay = 0f, Action onDestroyAction = null)
    {
        StartCoroutine(DestroyRoutine(gameObj, delay, onDestroyAction));
    }

    // 지연 파괴를 위한 코루틴
    private IEnumerator DestroyRoutine(GameObject gameObj, float delay = 0f, Action onDestroyAction = null)
    {
        if (delay != 0f)
        {
            yield return TimeManager.GetWaitForSeconds(delay);
        }
        var addEPrefab = goEprefabFinder[gameObj];
        if(addEPrefab.isPoolable)
        {
            yield break;
        }
        addEPrefab.isPoolable = true;
        addEPrefab.OnDestroy.Invoke();
        _poolableObjects[addEPrefab.eprefab].Push(addEPrefab);
        gameObj.SetActive(false);
        onDestroyAction?.Invoke();
        addEPrefab.destroyCoroutine = StartCoroutine(DestroyAfterDelay(addEPrefab));
    }

    // 일정 시간 후에 오브젝트를 파괴하는 코루틴
    private IEnumerator DestroyAfterDelay(AddEPrefab addEPrefab)
    {
        yield return TimeManager.GetWaitForSeconds(120f);
        if (addEPrefab.isPoolable)
        {
            var pool = _poolableObjects[addEPrefab.eprefab];
            if (pool.Contains(addEPrefab))
            {
                var tempStack = new Stack<AddEPrefab>(pool.Count);
                while (pool.Count > 0)
                {
                    var item = pool.Pop();
                    if (item != addEPrefab)
                    {
                        tempStack.Push(item);
                    }
                }

                while (tempStack.Count > 0)
                {
                    pool.Push(tempStack.Pop());
                }

                UnityEngine.GameObject.Destroy(addEPrefab.gameObject);

                if (!_preloadedPrefabs.Contains(addEPrefab.eprefab) && pool.Count == 0)
                {
                    ReleaseAsset(addEPrefab.eprefab);
                }
            }
        }
    }

    // 자식 오브젝트를 파괴하는 메서드
    public void DestroyWithChildren(GameObject gameObj, bool destroyOnlyChilden = false, float delay = 0f, Action onDestroyAction = null)
    {
        StartCoroutine(DestroyWithChildrenRoutine(gameObj, destroyOnlyChilden, delay, onDestroyAction));
    }

    // 자식 오브젝트 파괴를 위한 코루틴
    private IEnumerator DestroyWithChildrenRoutine(GameObject gameObj, bool destroyOnlyChilden = false, float delay = 0f, Action onDestroyAction = null)
    {
        if (delay != 0f)
        {
            yield return TimeManager.GetWaitForSeconds(delay);
        }
        var addEPrefabs = gameObj.GetComponentsInChildren<AddEPrefab>(true);
        for (int i = 0; i < addEPrefabs.Length; i++)
        {
            if (destroyOnlyChilden && i == 0)
            {
                continue;
            }
            if(addEPrefabs[i].isPoolable)
            {
                continue;
            }
            addEPrefabs[i].isPoolable = true;
            addEPrefabs[i].OnDestroy?.Invoke();
            _poolableObjects[addEPrefabs[i].eprefab].Push(addEPrefabs[i]);
            addEPrefabs[i].gameObject.SetActive(false);
        }
        onDestroyAction?.Invoke();
    }
    
    // 자산을 해제하는 메서드
    private void ReleaseAsset(EPrefab ePrefab)
    {
        if (_originPrefabs.ContainsKey(ePrefab))
        {
            Addressables.Release(_originPrefabs[ePrefab]);
            _originPrefabs.Remove(ePrefab);
        }
    }
    
    // PoolableManager가 파괴될 때 실행되는 메서드
    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();

        var dictkeyList = _originPrefabs.Keys.ToList();
        foreach (var ePrefab in dictkeyList)
        {
            ReleaseAsset(ePrefab);
        }
        _originPrefabs.Clear();
        Extensions.componentCache.Clear();
        Instance = null;
    }
}