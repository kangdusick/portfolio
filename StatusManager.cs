using CodeStage.AntiCheat.ObscuredTypes;
using Cysharp.Threading.Tasks;
using SRF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

// 상태 타입 열거형
public enum EStatusType
{
    baseValue = 0, // 기본값, 예: 공격력 +5, 공격력 -5
    multiplier = 1, // 증폭, 예: 공격력 5% 증폭
    divider = 2, // 억제, 예: 쿨타임 5% 억제
    adder = 3, // 추가, 예: 쿨타임 +5%, 쿨타임 -5%
}

/*
  상태 타입에 따른 계산 방식 설명:
  - multiplier: 최종 곱연산
  - divider: 최종 곱연산의 역수
  - adder: 합연산
  - baseValue: 기본값

  예시) 기본 쿨타임 60초
  쿨타임 +100% adder
  쿨타임 -50% adder
  쿨타임 30% 증폭 multiplier
  쿨타임 300% 억제 divider
  최종쿨타임 = (60)*(1 +  (100-50)*0.01 )*1.3/4 = 29.25초
*/

// 상태 딕셔너리 클래스
public class StatusDictionary : ObservableDictionary<(ELanguageTable status, EStatusType statusType), ObscuredFloat>
{
    public static Dictionary<int, StatusDictionary> statusDictionaryDict = new();
    public const string linkKey = "statusDictDesc";
    private static int statusDictIndex;
    private Dictionary<(ELanguageTable status, EStatusType statusType), (StatusDictionary refDict, float refDictMultiplier)> referenceStatusDictionary = new();
    private ObscuredFloat baseValue;
    private ObscuredFloat multiplier;
    private ObscuredFloat divider;
    private ObscuredFloat adder;

    private ObscuredFloat baseValueByReference;
    private ObscuredFloat multiplierByReference;
    private ObscuredFloat dividerByReference;
    private ObscuredFloat adderByReference;

    private Timer randomizeKeyTimer;

    private int dictIndex;
    public ObscuredFloat subValue { get; private set; } // 최종 값: 공격속도, subValue: 공격주기
    public ObscuredFloat FinalValue { get; private set; }
    Func<ObscuredFloat, ObscuredFloat> setSubValueFunc;
    private ELanguageTable subValueDesc;
    public string SubValueDescription
    {
        get
        {
            string replacement = $"<link={$"{linkKey}{dictIndex}"}><u>{subValue.KMBTUnit()}</u></link>";
            return replacement;
        }
    }
    public string FinalValueDescription
    {
        get
        {
            string replacement = $"<link={$"{linkKey}{dictIndex}"}><u>{FinalValue.KMBTUnit()}</u></link>";
            return replacement;
        }
    }

    // 키를 무작위로 변경하는 루틴
    private void RandomizeKeyRoutine()
    {
        baseValue.RandomizeCryptoKey();
        multiplier.RandomizeCryptoKey();
        divider.RandomizeCryptoKey();
        adder.RandomizeCryptoKey();
        baseValueByReference.RandomizeCryptoKey();
        multiplierByReference.RandomizeCryptoKey();
        dividerByReference.RandomizeCryptoKey();
        adderByReference.RandomizeCryptoKey();
    }

    // 설명 문자열을 반환
    public string TmpLinkDesc
    {
        get
        {
            StringBuilder stringBuilder = new StringBuilder();
            var baseList = new List<(ELanguageTable eStatus, float value)>();
            var multiList = new List<(ELanguageTable eStatus, float value)>();
            var dividerList = new List<(ELanguageTable eStatus, float value)>();
            var adderList = new List<(ELanguageTable eStatus, float value)>();
            foreach (var item in dictionary)
            {
                if (Mathf.Abs(item.Value) <= 0.01f)
                {
                    continue;
                }
                switch (item.Key.statusType)
                {
                    case EStatusType.baseValue:
                        baseList.Add((item.Key.status, item.Value));
                        break;
                    case EStatusType.multiplier:
                        multiList.Add((item.Key.status, item.Value));
                        break;
                    case EStatusType.divider:
                        dividerList.Add((item.Key.status, item.Value));
                        break;
                    case EStatusType.adder:
                        adderList.Add((item.Key.status, item.Value));
                        break;
                }
            }
            foreach (var item in referenceStatusDictionary)
            {
                switch (item.Key.statusType)
                {
                    case EStatusType.baseValue:
                        if (Mathf.Abs(item.Value.refDict.FinalValue * item.Value.refDictMultiplier) <= 0.01f)
                        {
                            continue;
                        }
                        baseList.Add((item.Key.status, item.Value.refDict.FinalValue * item.Value.refDictMultiplier));
                        break;
                    case EStatusType.multiplier:
                        if (Mathf.Abs(item.Value.refDict.FinalValue * item.Value.refDictMultiplier - 1f) <= 0.01f)
                        {
                            continue;
                        }
                        multiList.Add((item.Key.status, (item.Value.refDict.FinalValue * item.Value.refDictMultiplier - 1f) * 100f));
                        break;
                    case EStatusType.divider:
                        if (Mathf.Abs(item.Value.refDict.FinalValue * item.Value.refDictMultiplier - 1f) <= 0.01f)
                        {
                            continue;
                        }
                        dividerList.Add((item.Key.status, (item.Value.refDict.FinalValue * item.Value.refDictMultiplier - 1f) * 100f));
                        break;
                    case EStatusType.adder:
                        if (Mathf.Abs(item.Value.refDict.FinalValue * item.Value.refDictMultiplier) <= 0.01f)
                        {
                            continue;
                        }
                        adderList.Add((item.Key.status, item.Value.refDict.FinalValue * item.Value.refDictMultiplier));
                        break;
                }
            }
            var comparison = new Comparison<(ELanguageTable eStatus, float value)>((a, b) => b.value.CompareTo(a.value));
            baseList.Sort(comparison);
            multiList.Sort(comparison);
            dividerList.Sort(comparison);
            adderList.Sort(comparison);
            stringBuilder.Append("<align=left>");

            stringBuilder.Append($"{ELanguageTable.FinalValue.LocalIzeText()}: {FinalValue.KMBTUnit()}\n");
            if (subValueDesc != ELanguageTable.valueTypeDefine)
            {
                stringBuilder.Append($"{subValueDesc.LocalIzeText(subValue.KMBTUnit())}\n");
            }

            stringBuilder.Append($"\n{ELanguageTable.BasicValue.LocalIzeText()}\n");
            if (baseList.Count > 0)
            {
                for (int i = 0; i < baseList.Count; i++)
                {
                    stringBuilder.Append($"{baseList[i].value.KMBTUnit(true)}({Extensions.LocalIzeText(baseList[i].eStatus.ToString())}) ");
                }
                stringBuilder.Append($" = {(baseValue + baseValueByReference).KMBTUnit()}\n");
            }
            stringBuilder.Append($"\n{ELanguageTable.Increase.LocalIzeText()}\n");
            if (adderList.Count > 0)
            {
                for (int i = 0; i < adderList.Count; i++)
                {
                    stringBuilder.Append($"{adderList[i].value.KMBTUnit(true)}%({Extensions.LocalIzeText(adderList[i].eStatus.ToString())}) ");
                }
                stringBuilder.Append($" = {(adder + adderByReference).KMBTUnit(true)}%\n");
            }
            stringBuilder.Append($"\n{ELanguageTable.Amplify.LocalIzeText()}\n");
            if (multiList.Count > 0)
            {
                for (int i = 0; i < multiList.Count; i++)
                {
                    stringBuilder.Append($"x{(1f + multiList[i].value * 0.01f).KMBTUnit()}({Extensions.LocalIzeText(multiList[i].eStatus.ToString())}) ");
                }
                stringBuilder.Append($" = x{(multiplier * multiplierByReference).KMBTUnit()}\n");
            }
            stringBuilder.Append($"\n{ELanguageTable.Suppress.LocalIzeText()}\n");
            if (dividerList.Count > 0)
            {
                for (int i = 0; i < dividerList.Count; i++)
                {
                    stringBuilder.Append($"/{(1f + dividerList[i].value * 0.01f).KMBTUnit()}({Extensions.LocalIzeText(dividerList[i].eStatus.ToString())}) ");
                }
                stringBuilder.Append($" = /{(divider * dividerByReference).KMBTUnit()}");
            }
            stringBuilder.Append("</align>");
            return stringBuilder.ToString();
        }
    }

    // 인덱서를 재정의하여 상태 값을 관리
    public override ObscuredFloat this[(ELanguageTable status, EStatusType statusType) key]
    {
        get
        {
            if (!base.ContainsKey(key))
            {
                base[key] = 0f;
            }
            return base[key];
        }
        set
        {
            var beforeValue = 0f;
            if (dictionary.ContainsKey(key))
            {
                beforeValue = dictionary[key];
            }
            SetFinalValue_Base(key.statusType, beforeValue, value);
            base[key] = value;
        }
    }

    // 상태 딕셔너리 생성자
    public StatusDictionary(Func<ObscuredFloat, ObscuredFloat> setSubValue = null, ELanguageTable subValueDesc = ELanguageTable.valueTypeDefine)
    {
        dictIndex = Interlocked.Increment(ref statusDictIndex);
        statusDictionaryDict.Add(dictIndex, this);
        baseValue = 0f;
        multiplier = 1f;
        divider = 1f;
        adder = 0f;

        baseValueByReference = 0f;
        multiplierByReference = 1f;
        dividerByReference = 1f;
        adderByReference = 0f;
        this.setSubValueFunc = setSubValue;
        this.subValueDesc = subValueDesc;
        SetFinalValue();
        randomizeKeyTimer = new Timer(_ => RandomizeKeyRoutine(), null, 0, 1000);
    }

    // 타이머를 멈추는 메서드
    public void StopRandomizeKeyTimer()
    {
        randomizeKeyTimer?.Change(Timeout.Infinite, 0);
    }

    // 상태 딕셔너리를 삭제하는 메서드
    public void DeleteStatusDict()
    {
        foreach (var item in referenceStatusDictionary)
        {
            item.Value.refDict.OnChanged.RemoveListener(SetFinalValue_Reference);
        }
        StopRandomizeKeyTimer();
    }

    // 모든 상태 딕셔너리를 초기화
    public static void AllClearStatusDict()
    {
        try
        {
            var a = statusDictionaryDict.ToArray();
            for (int i = 0; i < a.Length; i++)
            {
                a[i].Value.DeleteStatusDict();
            }
            statusDictionaryDict.Clear();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }

    // 참조 상태 딕셔너리를 추가
    public void AddReferenceStatusDict((ELanguageTable status, EStatusType statusType) key, StatusDictionary statusDictionary, float referenceDictionaryMultiflier = 1f)
    {
        referenceStatusDictionary[key] = (statusDictionary, referenceDictionaryMultiflier);
        statusDictionary.OnChanged.RemoveListener(SetFinalValue_Reference);
        statusDictionary.OnChanged.AddListener(SetFinalValue_Reference);
        SetFinalValue_Reference();
    }

    // 참조 상태 딕셔너리를 제거
    public void RemoveReferenceStatusDict((ELanguageTable status, EStatusType statusType) key, StatusDictionary statusDictionary)
    {
        if (referenceStatusDictionary.ContainsKey(key))
        {
            referenceStatusDictionary.Remove(key);
        }
        statusDictionary.OnChanged.RemoveListener(SetFinalValue_Reference);
        SetFinalValue_Reference();
    }

    // 참조 상태 딕셔너리에 따라 최종 값을 설정
    private void SetFinalValue_Reference()
    {
        baseValueByReference = 0f;
        multiplierByReference = 1f;
        dividerByReference = 1f;
        adderByReference = 0f;

        foreach (var item in referenceStatusDictionary)
        {
            switch (item.Key.statusType)
            {
                case EStatusType.baseValue:
                    baseValueByReference += (item.Value.refDict.FinalValue * item.Value.refDictMultiplier);
                    break;
                case EStatusType.multiplier:
                    multiplierByReference *= (item.Value.refDict.FinalValue * item.Value.refDictMultiplier);
                    break;
                case EStatusType.divider:
                    dividerByReference *= (item.Value.refDict.FinalValue * item.Value.refDictMultiplier);
                    break;
                case EStatusType.adder:
                    adderByReference += (item.Value.refDict.FinalValue * item.Value.refDictMultiplier);
                    break;
            }
        }

        SetFinalValue();
    }

    // 상태 타입에 따른 최종 값을 설정
    private void SetFinalValue_Base(EStatusType statusType, float beforeValue, float afterValue)
    {
        if (beforeValue == afterValue)
        {
            return;
        }
        switch (statusType)
        {
            case EStatusType.baseValue:
                baseValue += afterValue - beforeValue;
                break;
            case EStatusType.multiplier:
                multiplier = multiplier * (1f + afterValue * 0.01f) / (1f + beforeValue * 0.01f);
                break;
            case EStatusType.divider:
                divider = divider * (1f + afterValue * 0.01f) / (1f + beforeValue * 0.01f);
                break;
            case EStatusType.adder:
                adder += afterValue - beforeValue;
                break;
        }

        SetFinalValue();
    }

    // 최종 값을 설정
    private void SetFinalValue()
    {
        FinalValue = (baseValue + baseValueByReference) * (1f + (adder + adderByReference) * 0.01f) * multiplier * multiplierByReference / (divider * dividerByReference);
        if (!ReferenceEquals(setSubValueFunc, null))
        {
            subValue = setSubValueFunc(FinalValue);
        }
        OnChanged.Invoke();
    }

    protected override void Changed()
    {
    }

    // 상태 값에 따른 설명을 반환
    public static string GetDescriptionValue(float finalAmount, int statusType, bool isShowSign = false)
    {
        return GetDescriptionValue(finalAmount, (EStatusType)statusType, isShowSign);
    }

    // 상태 값에 따른 설명을 반환
    public static string GetDescriptionValue(float finalAmount, EStatusType statusType, bool isShowSign = false)
    {
        string sign = isShowSign && finalAmount > 0 ? "+" : "";

        switch (statusType)
        {
            case EStatusType.baseValue:
                return $"{sign}{finalAmount.KMBTUnit()}";
            case EStatusType.multiplier:
                return $"{sign}{finalAmount.KMBTUnit()}% {ELanguageTable.Amplify.LocalIzeText()}";
            case EStatusType.divider:
                return $"{sign}{finalAmount.KMBTUnit()}% {ELanguageTable.Suppress.LocalIzeText()}";
            case EStatusType.adder:
                return $"{sign}{finalAmount.KMBTUnit()}%";
            default:
                return string.Empty;
        }
    }

    // 파워 테이블과 상태 값에 따른 설명을 반환
    public static string GetPowerDescription(EPowerTable ePowerTable, float finalAmount, ECharacterTable eCharacterTable)
    {
        var powerTable = TableManager.PowerTableDict[ePowerTable];
        var eStatusType = (EStatusType)powerTable.multiplyType;
        var stringBuilder = new StringBuilder();
        switch (ePowerTable)
        {
            case EPowerTable.WantedCriminalBounty:
            case EPowerTable.GoldenOrgeBounty:
            case EPowerTable.EliteOrcPerWave:
            case EPowerTable.BountyChestIncrease:
                {
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount * FarmStatus.FarmBountyMultiplierDict.FinalValue, eStatusType, false)));
                    break;
                }
            case EPowerTable.SummonMotherChicken:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.MotherChicken, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription));
                    break;
                }
            case EPowerTable.SummonGoldenSheep:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.GoldenSheep, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription, characterStatus.ProducePerCycleDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.SummonTurkey:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.Turkey, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription));
                    break;
                }
            case EPowerTable.SummonGoldenChicken:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.GoldChicken, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription, characterStatus.ProducePerCycleDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.SummonWhiteLeopard:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.WhitePanther, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription, characterStatus.ProducePerCycleDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.SummonBundle:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.BundleOrigin, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.ProductionSpeedDict.SubValueDescription, characterStatus.ProducePerCycleDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.SummonBattleGolem:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.BattleGolem, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.attackSpeedDict.SubValueDescription, characterStatus.attackDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.SummonGolem:
                {
                    var characterStatus = new CharacterStatus();
                    characterStatus.Init(ECharacterTable.Golem, 0, 0);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(characterStatus.attackSpeedDict.SubValueDescription, characterStatus.attackDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.GrainAcquisition:
            case EPowerTable.MilkAcquisition:
                {
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount * FarmStatus.StageRewardAmpDict.FinalValue, eStatusType, true)));
                    break;
                }
            case EPowerTable.FenceCounterattack:
                {
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount * FarmStatus.characterStatusDict[ECharacterTable.Wall].hpDict.FinalValue, eStatusType, true)));
                    break;
                }
            case EPowerTable.Money:
            case EPowerTable.Loan:
                {
                    var descDict = new StatusDictionary();
                    descDict[(ELanguageTable.DefaultValue, EStatusType.baseValue)] = finalAmount;
                    descDict.AddReferenceStatusDict((ELanguageTable.StageLvInflation, EStatusType.multiplier), FarmStatus.StageInflationDict);
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(descDict.FinalValueDescription));
                    break;
                }
            case EPowerTable.StoneThrowChanceOnFeeding:
                stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount, eStatusType, true), FarmStatus.KingStoneDamageDict.FinalValueDescription));
                break;
            case EPowerTable.ExtraBulletChance100CooldownSpeed:
                {
                    var descDict = new StatusDictionary((finalValue) =>
                    {
                        var cooldown = 1f / finalValue;
                        return cooldown;
                    }, ELanguageTable.subValueDesc_RafidFirePeriod);
                    descDict[(ELanguageTable.DefaultValue, EStatusType.baseValue)] = finalAmount;
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(descDict.SubValueDescription));
                    break;
                }
            default:
                if (KMP.KMPSearch(TableManager.LanguageTableDict[powerTable.desckLanguageKey.ParseEnum<ELanguageTable>()].kr, "{1}").Count >= 1)
                {
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount, eStatusType, true), TableManager.CharacterTableDict[eCharacterTable].nameLanguageKey.LocalIzeText()));
                    break;
                }
                else
                {
                    stringBuilder.Append(powerTable.desckLanguageKey.LocalIzeText(GetDescriptionValue(finalAmount, eStatusType, true)));
                    break;
                }
        }
        return stringBuilder.ToString();
    }

    // 특정 상태 딕셔너리에 파워를 적용
    private static void ApplyPowerInner(StatusDictionary targetStatusDict, (ELanguageTable, EStatusType) dictKey, float finalAmount, bool isAdder)
    {
        if (isAdder)
        {
            targetStatusDict[dictKey] += finalAmount;
        }
        else
        {
            targetStatusDict[dictKey] = finalAmount;
        }
    }

    // 캐릭터 상태에 파워를 적용
    public static void ApplyPower(CharacterStatus targetCharacterStatus, EPowerTable ePowerTable, ELanguageTable eStatus, float justPowerAmount, bool isAdder)
    {
        var eStatusType = (EStatusType)TableManager.PowerTableDict[ePowerTable].multiplyType;
        var finalAmount = justPowerAmount * TableManager.PowerTableDict[ePowerTable].amount;

        switch (ePowerTable)
        {
            case EPowerTable.AllMonsterHealth:
            case EPowerTable.AllMonsterHealthIncrease:
            case EPowerTable.AllMonsterHealthAmplification:
            case EPowerTable.AllMonsterHealthSuppression:
                ApplyPowerInner(FarmStatus.AllMonsterHpMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmHp:
            case EPowerTable.FarmHpIncrease:
            case EPowerTable.FarmHpAmplification:
            case EPowerTable.FarmHpSuppression:
                ApplyPowerInner(FarmStatus.FarmHpDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmDefense:
            case EPowerTable.FarmDefenseIncrease:
            case EPowerTable.FarmDefenseAmplification:
            case EPowerTable.FarmDefenseSuppression:
                ApplyPowerInner(FarmStatus.FarmDefenceDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.PetMovementSpeed:
            case EPowerTable.PetMovementSpeedIncrease:
            case EPowerTable.PetMovementSpeedAmplification:
            case EPowerTable.PetMovementSpeedSuppression:
                ApplyPowerInner(targetCharacterStatus.moveSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllPetMovementSpeed:
            case EPowerTable.AllPetMovementSpeedIncrease:
            case EPowerTable.AllPetMovementSpeedAmplification:
            case EPowerTable.AllPetMovementSpeedSuppression:
                ApplyPowerInner(FarmStatus.AllPetMoveSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllMonsterMovementSpeed:
            case EPowerTable.AllMonsterMovementSpeedIncrease:
            case EPowerTable.AllMonsterMovementSpeedAmplification:
            case EPowerTable.AllMonsterMovementSpeedSuppression:
                ApplyPowerInner(FarmStatus.AllMonsterMoveSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllMonsterDefense:
            case EPowerTable.AllMonsterDefenseIncrease:
            case EPowerTable.AllMonsterDefenseAmplification:
            case EPowerTable.AllMonsterDefenseSuppression:
                ApplyPowerInner(FarmStatus.AllMonsterDeffenceDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FenceDurabilityRecoverySpeed:
            case EPowerTable.FenceDurabilityRecoverySpeedIncrease:
            case EPowerTable.FenceDurabilityRecoverySpeedAmplification:
            case EPowerTable.FenceDurabilityRecoverySpeedSuppression:
                ApplyPowerInner(FarmStatus.characterStatusDict[ECharacterTable.Wall].hpRegenDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllMonsterHealthRecoverySpeed:
            case EPowerTable.AllMonsterHealthRecoverySpeedIncrease:
            case EPowerTable.AllMonsterHealthRecoverySpeedAmplification:
            case EPowerTable.AllMonsterHealthRecoverySpeedSuppression:
                break;
            case EPowerTable.AllMonsterAttackPower:
            case EPowerTable.AllMonsterAttackPowerIncrease:
            case EPowerTable.AllMonsterAttackPowerAmplification:
            case EPowerTable.AllMonsterAttackPowerSuppression:
                ApplyPowerInner(FarmStatus.AllMonsterAttackMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmAttackPower:
            case EPowerTable.FarmAttackPowerIncrease:
            case EPowerTable.FarmAttackPowerAmplification:
            case EPowerTable.FarmAttackPowerSuppression:
                ApplyPowerInner(FarmStatus.FarmAttackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmAttackSpeed:
            case EPowerTable.FarmAttackSpeedIncrease:
            case EPowerTable.FarmAttackSpeedAmplification:
            case EPowerTable.FarmAttackSpeedSuppression:
                ApplyPowerInner(FarmStatus.FarmAttackSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.PetAttackSpeed:
            case EPowerTable.PetAttackSpeedIncrease:
            case EPowerTable.PetAttackSpeedAmplification:
            case EPowerTable.PetAttackSpeedSuppression:
                ApplyPowerInner(targetCharacterStatus.attackSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllPetAttackSpeed:
            case EPowerTable.AllPetAttackSpeedIncrease:
            case EPowerTable.AllPetAttackSpeedAmplification:
            case EPowerTable.AllPetAttackSpeedSuppression:
                ApplyPowerInner(FarmStatus.AllPetAttackSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.BulletPower:
            case EPowerTable.BulletPowerIncrease:
            case EPowerTable.BulletPowerAmplification:
            case EPowerTable.BulletPowerSuppression:
                ApplyPowerInner(FarmStatus.bulletDamage, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.PetPower:
            case EPowerTable.PetPowerIncrease:
            case EPowerTable.PetPowerAmplification:
            case EPowerTable.PetPowerSuppression:
                ApplyPowerInner(targetCharacterStatus.attackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllPetPower:
            case EPowerTable.AllPetPowerIncrease:
            case EPowerTable.AllPetPowerAmplification:
            case EPowerTable.AllPetPowerSuppression:
                ApplyPowerInner(FarmStatus.AllPetAttackMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.GrenadeAttackPower:
            case EPowerTable.GrenadeAttackPowerIncrease:
            case EPowerTable.GrenadeAttackPowerAmplification:
            case EPowerTable.GrenadeAttackPowerSuppression:
                ApplyPowerInner(FarmStatus.grenadeDamageDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmProduction:
            case EPowerTable.FarmProductionIncrease:
            case EPowerTable.FarmProductionAmplification:
            case EPowerTable.FarmProductionSuppression:
                ApplyPowerInner(FarmStatus.FarmProductionMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ProductionSpeed:
            case EPowerTable.ProductionSpeedIncrease:
            case EPowerTable.ProductionSpeedAmplification:
            case EPowerTable.ProductionSpeedSuppression:
                ApplyPowerInner(targetCharacterStatus.ProductionSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmProductionSpeed:
            case EPowerTable.FarmProductionSpeedIncrease:
            case EPowerTable.FarmProductionSpeedAmplification:
            case EPowerTable.FarmProductionSpeedSuppression:
                ApplyPowerInner(FarmStatus.FarmProductionSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.Debt:
                GameManager.Instance.GameDebt += (finalAmount * FarmStatus.StageInflationDict.FinalValue);
                break;
            case EPowerTable.DebtIncrease:
            case EPowerTable.DebtAmplification:
                GameManager.Instance.GameDebt *= (1f + finalAmount * 0.01f);
                break;
            case EPowerTable.DebtSuppression:
                GameManager.Instance.GameDebt /= (1f + finalAmount * 0.01f);
                break;
            case EPowerTable.ChickenProduction:
            case EPowerTable.ChickenProductionIncrease:
            case EPowerTable.ChickenProductionAmplification:
            case EPowerTable.ChickenProductionSuppression:
                ApplyPowerInner(FarmStatus.ChickenProductionMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.LeopardProduction:
            case EPowerTable.LeopardProductionIncrease:
            case EPowerTable.LeopardProductionAmplification:
            case EPowerTable.LeopardProductionSuppression:
                ApplyPowerInner(FarmStatus.PantherProductionMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmBounty:
            case EPowerTable.FarmBountyIncrease:
            case EPowerTable.FarmBountyAmplification:
            case EPowerTable.FarmBountySuppression:
                ApplyPowerInner(FarmStatus.FarmBountyMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmBountyProduction:
            case EPowerTable.FarmBountyProductionIncrease:
            case EPowerTable.FarmBountyProductionAmplification:
            case EPowerTable.FarmBountyProductionSuppression:
                ApplyPowerInner(FarmStatus.FarmProductionMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                ApplyPowerInner(FarmStatus.FarmBountyMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FeedingRate:
            case EPowerTable.FeedingRateIncrease:
            case EPowerTable.FeedingRateAmplification:
            case EPowerTable.FeedingRateSuppression:
                ApplyPowerInner(targetCharacterStatus.FeedingSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmFeedingRate:
            case EPowerTable.FarmFeedingRateIncrease:
            case EPowerTable.FarmFeedingRateAmplification:
            case EPowerTable.FarmFeedingRateSuppression:
                ApplyPowerInner(FarmStatus.FeedingSpeedMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AllLivestockMaximumSatiety:
            case EPowerTable.AllLivestockMaximumSatietyIncrease:
            case EPowerTable.AllLivestockMaximumSatietyAmplification:
            case EPowerTable.AllLivestockMaximumSatietySuppression:
                ApplyPowerInner(FarmStatus.MaxSatietyMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FeedingSatiety:
            case EPowerTable.FeedingSatietyIncrease:
            case EPowerTable.FeedingSatietyAmplification:
            case EPowerTable.FeedingSatietySuppression:
                ApplyPowerInner(FarmStatus.FeedSatietyDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.GrenadeCost:
            case EPowerTable.GrenadeCostIncrease:
            case EPowerTable.GrenadeCostAmplification:
            case EPowerTable.GrenadeCostSuppression:
                ApplyPowerInner(InGameUIButton.inGameUIButtonDict[InGameUIButton.EInGemeUIType.GrenadeMode].costDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.GrenadeArea:
            case EPowerTable.GrenadeAreaIncrease:
            case EPowerTable.GrenadeAreaAmplification:
            case EPowerTable.GrenadeAreaSuppression:
                ApplyPowerInner(FarmStatus.GranadeRangeDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.BulletSize:
            case EPowerTable.BulletSizeIncrease:
            case EPowerTable.BulletSizeAmplification:
            case EPowerTable.BulletSizeSuppression:
                ApplyPowerInner(FarmStatus.BulletSizeMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.OreDamage:
            case EPowerTable.OreDamageIncrease:
            case EPowerTable.OreDamageAmplification:
            case EPowerTable.OreDamageSuppression:
                ApplyPowerInner(FarmStatus.oreStoneDamageMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.WantedCriminalBounty:
                GameManager.Instance.SpawnMonster(GameManager.Instance.stageInfo.monsterList.Random().ParseEnum<ECharacterTable>(), true);
                break;
            case EPowerTable.GoldenOrgeBounty:
                GameManager.Instance.SpawnMonster(ECharacterTable.GoldenOgre, true);
                break;
            case EPowerTable.EliteOrcPerWave:
                GameManager.Instance.isEliteOrcSpawnOn = true;
                break;
            case EPowerTable.CurrentMoneyAmplification:
                GameManager.Instance.GainMoney(GameManager.Instance.CurrentMoney * finalAmount * 0.01f);
                break;
            case EPowerTable.Money:
                if (GameManager.isInGame)
                {
                    GameManager.Instance.GainMoneyByInflation(finalAmount);
                }
                break;
            case EPowerTable.Loan:
                if (GameManager.isInGame)
                {
                    GameManager.Instance.GainMoneyByInflation(finalAmount);
                    GameManager.Instance.GameDebt += finalAmount * FarmStatus.StageInflationDict.FinalValue;
                }
                break;
            case EPowerTable.StoneThrowChanceOnFeeding:
                var addedDict = AddNewStatusRoutine(FarmStatus.KingStoneChanceDictList, (eStatus, eStatusType), finalAmount, isAdder);
                if (addedDict != null)
                {
                    addedDict.AddReferenceStatusDict((ELanguageTable.KingStoneChanceMultiplierDict, EStatusType.multiplier), FarmStatus.KingStoneChanceMultiplierDict);
                }
                break;
            case EPowerTable.GrenadeChanceOnShoot:
                ApplyPowerInner(FarmStatus.GranadeChanceWhenAttackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ExtraBulletChanceOnShoot:
                AddNewStatusRoutine(FarmStatus.ExtraBulletChanceDictList, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.BulletPenetrationChance:
                AddNewStatusRoutine(FarmStatus.BulletPenetrationChanceDictList, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ExtraBulletChance100CooldownSpeed:
                ApplyPowerInner(FarmStatus.ExtraBulletChance100CooldownSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.GrainAcquisition:
                if (GameManager.isInGame)
                {
                    GameManager.Instance.InGameExtraGrain += (finalAmount * FarmStatus.StageRewardAmpDict.FinalValue);
                }
                break;
            case EPowerTable.MilkAcquisition:
                if (GameManager.isInGame)
                {
                    GameManager.Instance.InGameExtraMilk += (finalAmount * FarmStatus.StageRewardAmpDict.FinalValue);
                }
                break;
            case EPowerTable.AdditionalWaveOpportunities:
                InGameUIButton.inGameUIButtonDict[InGameUIButton.EInGemeUIType.AdditionalWave].gameObject.SetActive(true);
                ApplyPowerInner(FarmStatus.AdditionalWaveDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FenceCounterattack:
                ApplyPowerInner(FarmStatus.HpBaseCounterAttackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.BountyChestIncrease:
                ApplyPowerInner(CollisionDetectManager.Instance.petList.Find(x => x.eCharacterTable == ECharacterTable.BountyChestOrigin).characterStatus.BountyDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.BulletCriticalChance:
                ApplyPowerInner(FarmStatus.BulletCriticalChance, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.SummonGolem:
                PoolableManager.Instance.InstantiateAsync<Pet>(EPrefab.Golem).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.Golem);
                });
                break;
            case EPowerTable.SummonBattleGolem:
                PoolableManager.Instance.InstantiateAsync<Pet>(EPrefab.BattleGolem).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.BattleGolem);
                });
                break;
            case EPowerTable.SummonTurkey:
                PoolableManager.Instance.InstantiateAsync<Flock>(EPrefab.Turkey).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.Turkey);
                });
                break;
            case EPowerTable.SummonGoldenSheep:
                PoolableManager.Instance.InstantiateAsync<Pet>(EPrefab.GoldenSheep).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.GoldenSheep);
                });
                break;
            case EPowerTable.SummonGoldenChicken:
                PoolableManager.Instance.InstantiateAsync<Flock>(EPrefab.GoldChicken).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.GoldChicken);
                });
                break;
            case EPowerTable.SummonWhiteLeopard:
                PoolableManager.Instance.InstantiateAsync<Flock>(EPrefab.WhitePanther).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.WhitePanther);
                });
                break;
            case EPowerTable.SummonMotherChicken:
                PoolableManager.Instance.InstantiateAsync<MotherChicken>(EPrefab.MotherChicken).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.MotherChicken);
                });
                break;
            case EPowerTable.SummonBundle:
                PoolableManager.Instance.InstantiateAsync<Pet>(EPrefab.BundleOrigin).ContinueWithNullCheck(x =>
                {
                    x.Init(ECharacterTable.BundleOrigin);
                });
                break;
            case EPowerTable.PoisonRain:
                PoolableManager.Instance.InstantiateAsync<PoisonRain>(EPrefab.PoisonRain, Vector3.up * 16.2f, new Vector3(0.075f, 0.5f, 1f)).ContinueWithNullCheck(x =>
                {
                    x.Init();
                });
                break;
            case EPowerTable.PetAttackPowerProportionalToFarmAttack:
                ApplyPowerInner(targetCharacterStatus.attackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AutomaticProduceCollection:
                ApplyPowerInner(targetCharacterStatus.AutomaticProduceCollectionDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.MeatDropBounty:
                ApplyPowerInner(targetCharacterStatus.MeatDropBountyDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ButcheryProduction:
                ApplyPowerInner(targetCharacterStatus.ButcheryProductionDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ProducePerCycle:
                ApplyPowerInner(targetCharacterStatus.ProducePerCycleDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FarmHealthProportionalToPetHealth:
                ApplyPowerInner(targetCharacterStatus.hpDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.LivestockResuscitationSpeed:
                ApplyPowerInner(targetCharacterStatus.LivestockResuscitationSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.FenceRepairWhenAttack:
                ApplyPowerInner(targetCharacterStatus.FenceRepairWhenAttackDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AdditionalAbilityChance:
                ApplyPowerInner(FarmStatus.AdditionalAbilitySelectChanceDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AddStartAbility:
                ApplyPowerInner(FarmStatus.AdditionalStartAbilityDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.KingStoneChanceAmplify:
                ApplyPowerInner(FarmStatus.KingStoneChanceMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.CompoundInterest:
                ApplyPowerInner(FarmStatus.CompoundInterestDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.GrenadeSplitDamage:
                ApplyPowerInner(FarmStatus.GranadeSplitDamageMultiplierDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.ContinuousGrenadePurchase:
                ApplyPowerInner(FarmStatus.ContinuousGrenadePurchaseDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.PetNaturalHealingBasic:
                ApplyPowerInner(targetCharacterStatus.hpRegenDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.PetRevivalSpeedBasic:
                ApplyPowerInner(targetCharacterStatus.ReviveSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.AbilityDiceCost:
            case EPowerTable.AbilityDiceCostIncrease:
            case EPowerTable.AbilityDiceCostAmplification:
            case EPowerTable.AbilityDiceCostSuppression:
                ApplyPowerInner(FarmStatus.abilityRerollCostDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.NaturaldisasterSpeed:
            case EPowerTable.NaturaldisasterSpeedIncrease:
            case EPowerTable.NaturaldisasterSpeedAmplification:
            case EPowerTable.NaturaldisasterSpeedSuppression:
                ApplyPowerInner(FarmStatus.NaturaldisasterSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.TimeLimit:
                if (GameManager.isInGame)
                {
                    GameManager.Instance.LeftGameTime += finalAmount;
                }
                break;
            case EPowerTable.Production:
            case EPowerTable.ProductionIncrease:
            case EPowerTable.ProductionAmplification:
            case EPowerTable.ProductionSuppression:
                ApplyPowerInner(targetCharacterStatus.ProducePerCycleDict, (eStatus, eStatusType), finalAmount, isAdder);
                ApplyPowerInner(targetCharacterStatus.ButcheryProductionDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            case EPowerTable.MonsterAppearSpeed:
            case EPowerTable.MonsterAppearSpeedIncrease:
            case EPowerTable.MonsterAppearSpeedAmplification:
            case EPowerTable.MonsterAppearSpeedSuppression:
                ApplyPowerInner(FarmStatus.MonsterAppearSpeedDict, (eStatus, eStatusType), finalAmount, isAdder);
                break;
            default:
                break;
        }
    }

    // 새로운 상태 루틴을 추가
    private static StatusDictionary AddNewStatusRoutine(List<StatusDictionary> targetDictList, (ELanguageTable, EStatusType) dictKey, float value, bool isAdder)
    {
        StatusDictionary addedDict = null;
        if (value < 0f)
        {
            foreach (var item in targetDictList)
            {
                if (item.ContainsKey(dictKey) && Mathf.Abs(item[dictKey] + value) <= 0.0001f)
                {
                    targetDictList.Remove(item);
                    break;
                }
            }
        }
        else
        {
            addedDict = new StatusDictionary();
            targetDictList.Add(addedDict);
            ApplyPowerInner(targetDictList.Last(), dictKey, value, isAdder);
            targetDictList = targetDictList.OrderBy(x => x.FinalValue).ToList();
        }
        return addedDict;
    }
}
