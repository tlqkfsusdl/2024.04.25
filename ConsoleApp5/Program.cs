using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using static Character;

public class Program
{
    static bool isRunning = true; //isRunning 은 bool 입니다. 기본값은 참 입니다. (bool 은 참과 거짓 둘중하나를 선택하게 하는 함수) / bool 을 선언하는 이유 = 해당 메서드가 켜져있는동안 아래에 있는 while 문을 통하여 Console.WriteLine 을 켜놓고 있어야 하기 때문입니다.

    static void Main(string[] args)
    {
        Character character = new Character(); // Character 클래스를 character 로 가져옵니다.

        while (isRunning) // isRunning 이 true 가 유지되는 동안 {} 안에 있는 코드를 실행합니다. (while 은 ~하는 동안에 라는 뜻)
        {
            Console.WriteLine("\n스파르타 마을에 오신 것을 환영합니다.");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식");
            Console.WriteLine("6. 데이터 세이브");
            Console.WriteLine("7. 데이터 로드");
            Console.WriteLine("0. 게임 종료");

            Console.Write("\n원하시는 행동을 입력해주세요. >> ");
            string input = Console.ReadLine(); // 사용자가 문자열을 입력하는걸 input 에 할당합니다.
            // Console.ReadLine() 뜻 -> 사용자가 입력을 하고 엔터를 누를때까지 대기

            switch (input) // input 에는 해당 선택지들이 있습니다.
            {
                case "1": // "1" 문자열을 입력받는 경우에는
                    character.ShowStatus(); // character 의 ShowStatus메서드를 실행합니다. 메서드를 입력할때는 name() << 소괄호가 꼭 붙어야함.
                    break;
                case "2":
                    character.ShowInventory();
                    break;
                case "3":
                    character.ShowShop();
                    break;
                case "4":
                    character.EnterDungeon();
                    break;
                case "5":
                    character.Rest();
                    break;
                case "6":
                    SaveGame(); // 세이브 기능 추가
                    break;
                case "7":
                    LoadGame(); // 로드 기능 추가
                    break;
                case "0": // "0" 문자열을 입력받는 경우에는
                    isRunning = false; // isRunning 의 bool 값을 false 로 변경합니다.
                    break;
                default: // 위에 해당하지 않는 값을 입력받았다면
                    Console.WriteLine("\n잘못된 입력입니다."); // 해당 문구를 실행시킵니다.
                    break;
            }
        }
    }

    // SaveGame 메서드 JSON 버전
    static void SaveGame()
    {
        try
        {
            GameData gameData = new GameData
            {
                Gold = 500,
                Attack = 10,
                Defense = 5,
                Level = 1,
                Health = 100,
                Inventory = new List<Item>(),
                EquippedItems = new List<Item>(),
                DungeonCleared = 0
            };

            string jsonString = JsonSerializer.Serialize(gameData);
            File.WriteAllText("savedata.json", jsonString);
            Console.WriteLine("데이터를 세이브했습니다.");
            Console.WriteLine("데이터를 세이브했습니다." + gameData.Gold);
        }
        catch (Exception ex)
        {
            Console.WriteLine("세이브 중 오류가 발생했습니다: " + ex.Message);
            Console.WriteLine("스택 트레이스: " + ex.StackTrace);
        }
    }

    // LoadGame 메서드 JSON 버전
    static void LoadGame()
    {
        if (File.Exists("savedata.json"))
        {
            string jsonString = File.ReadAllText("savedata.json");
            GameData loadedData = JsonSerializer.Deserialize<GameData>(jsonString);
            Console.WriteLine("로드된 데이터:");
            Console.WriteLine("Gold: " + loadedData.Gold);
            Console.WriteLine("Attack: " + loadedData.Attack);
            // ... (다른 필드들에 대한 출력 추가)
            Console.WriteLine("데이터를 로드했습니다.");
        }
        else
        {
            Console.WriteLine("로드할 파일이 존재하지 않습니다.");
        }
    }
}

[Serializable]
public class GameData
{
    public float Gold { get; set; }
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float Level { get; set; }
    public float Health { get; set; }
    public List<Item> Inventory { get; set; }
    public List<Item> EquippedItems { get; set; }
    public float DungeonCleared { get; set; }
}

[Serializable]
public class Character
{
    Item item = new Item("아이템", 100, ItemType.Weapon);
    public float Gold = 500; // Gold 는 숫자입니다. 기본값은 500 입니다.
    public float Attack = 10; // Attack 은 숫자입니다. 기본값은 10 입니다.
    public float Defense = 5;
    public float Level = 1;
    public float Health = 100;
    public List<Item> Inventory = new List<Item>(); // Inventory 의 List<Item> 에 new List<Item>() 값을 보관합니다.
    public List<Item> EquippedItems = new List<Item>(); // EquippedItems 의 List<Item> 에 new List<Item>() 값을 보관합니다.
    public float DungeonCleared = 0; // DungeonCleared 는 숫자입니다. 기본값은 0 입니다. (던전 클리어 횟수를 카운트하는데 사용)

    public void ShowStatus() // ShowStatus 메서드가 실행될 때 {} 코드를 실행합니다.
    {
        float totalAttack = Attack; // totalAttack 은 숫자입니다. Attack 값을 기본값으로 사용합니다.
        float totalDefense = Defense;

        float PlusAttack = 0; // PlusAttack 은 숫자입니다. 기본값은 0 입니다.
        float PlusDefense = 0;

        foreach (var item in EquippedItems) // 장착한 아이템이 뭔지 찾는걸 반복합니다.
        {
            if (item is Weapon) // 만약 무기일경우
            {
                Weapon weapon = item as Weapon; // 무기 의 타입이 Weapon 인지 확인하고, 이를 형변환하여 weapon 변수에 할당합니다.
                totalAttack += weapon.AttackBonus; // totalAttack 에 무기의 공격력값을 더합니다. ( A += B -> A 값에 B 를 추가한다. )
                PlusAttack += weapon.AttackBonus;
            }
            else if (item is Armor)  // Armor라면 방어력 추가
            {
                Armor armor = item as Armor;
                totalDefense += armor.DefenseBonus;
                PlusDefense += armor.DefenseBonus;
            }
        }

        bool inStatus = true; // inStatus 는 bool 입니다. 기본값은 참 입니다.

        while (inStatus) // inStatus 이 true 가 유지되는 동안 {} 코드를 실행합니다.
        {
            Console.WriteLine("\n== 캐릭터의 정보 ==");
            Console.WriteLine("\nLv.  " + Level + "     박재우 ( 전사 )");
            Console.WriteLine("공격력 : " + totalAttack + " (+" + PlusAttack + ")");
            Console.WriteLine("방어력 :" + totalDefense + " (+" + PlusDefense + ")");
            Console.WriteLine("체력 : " + Health);
            Console.WriteLine("\nGold : " + Gold + " G");
            Console.WriteLine("\n0. 나가기");

            Console.Write("\n원하시는 행동을 입력해주세요. >> ");
            string statusInput = Console.ReadLine(); // 플레이어가 입력하는 값을 statusInput 에 문자열로 저장합니다.
            if (statusInput == "0") // 만약 0 을 입력 받을 경우
            {
                inStatus = false; // inStatus 의 bool 값을 거짓으로 바꿉니다.
            }
            else // 그 외에 입력 받을 경우
            {
                Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요."); // 를 출력합니다.
            }
        }
    }

    public void ShowInventory() // ShowInventory 메서드가 실행될 때 {} 코드를 실행합니다.
    {
        bool inventory = true; // inventory 는 bool 입니다. 기본값은 참 입니다.

        while (inventory) // inventory 이 true 가 유지되는 동안 {} 코드를 실행합ㄴ니다.
        {
            Console.WriteLine("\n== 인벤토리 ==");
            for (int i = 0; i < Inventory.Count; i++) // Inventory 리스트의 요소 개수를 기준으로 기본값 0부터 시작하여 하나씩 증가합니다. (아이템 추가)
            {
                string equippedMark = EquippedItems.Contains(Inventory[i]) ? "[E]" : ""; // equippedMark 는 문자열입니다. 기본값은 인벤토리내에 장착한 아이템일 경우 E 표시를 하도록 되어있습니다.
                Console.WriteLine($"{i + 1}. {equippedMark} {Inventory[i].Name}"); // 문자를 출력합니다. "기본값 + 1 (아이템 개수가 늘어나면 기본값도 늘어남)" \ {아이템 마크 표시 (장착중일때만 표시됨)} \ 아이템 이름" 순입니다.
            }
            Console.WriteLine("\n0. 나가기");

            Console.Write("\n원하시는 행동을 입력해주세요. >> ");
            string input = Console.ReadLine(); // 입력받은 값을 input에 저장합니다.
            if (input == "0") // 0을 입력받으면
            {
                inventory = false; // inventory 의 bool 값을 false 로 바꿉니다.
            }
            else // 그게 아니라면
            {
                float itemIndex; // itemIndex 는 숫자입니다.
                if (float.TryParse(input, out itemIndex) && itemIndex > 0 && itemIndex <= Inventory.Count) // 입력받은 숫자가 위 추가된 아이템들의 숫자와 동일할경우 해당 아이템을 장착합니다.
                {
                    EquipItem(itemIndex - 1); // 인덱스는 0부터 시작하므로 입력값에서 1을 빼줍니다.
                }
                else // 그것도 아니라면
                {
                    Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요."); // 해당 문구를 출력합니다.
                }
            }
        }
    }

    public void EquipItem(float itemIndex) // EquipItem 메서드가 실행될 때 {} 코드를 실행합니다.
    {
        if (itemIndex < 0 || itemIndex >= Inventory.Count) // itemIndex 가 잘못된 값이 들어왔을때
        {
            Console.WriteLine("itemIndex 값이 잘못되었습니다."); // 해당 문구를 출력하고
            return; // 해당 메서드를 종료시킵니다.
        }

        Item itemToEquip = Inventory[(int)itemIndex]; // Inventory 리스트에서 itemIndex에 해당하는 위치에 있는 아이템을 찾아서 itemToEquip 변수에 할당합니다. (int) 가 붙은 이유는 itemIndex 가 부동소수점으로 선언되어 있기 때문에 정수형으로 바꾸기 위하여 추가되었습니다.
        if (itemToEquip is Armor armorToEquip) // 만약 itemToEquip 가 Armor 라면
        {
            var equippedItem = EquippedItems.OfType<Armor>().FirstOrDefault(item => item.Slot == armorToEquip.Slot); // 아이템 슬룻 armorToEquip 에 장착합니다.
            if (equippedItem != null) // 만약 이미 장착중인 아이템이 있을 경우
            {
                EquippedItems.Remove(equippedItem); // 장착된 아이템을 교체합니다.
                Console.WriteLine($"\n{equippedItem.Name} 장착해제함."); // 교체하고 난 뒤 해당 문구를 출력합니다.
            }
        }
        else if (itemToEquip is Weapon weaponToEquip) // 그게 아니라 itemToEquip 가 Weapon 라면
        {
            var equippedItem = EquippedItems.OfType<Weapon>().FirstOrDefault(item => item.Slot == weaponToEquip.Slot); // 아이템 슬룻 weaponToEquip 에 장착합니다.
            if (equippedItem != null) // 만약 이미 장착중인 아이템이 있을 경우
            {
                EquippedItems.Remove(equippedItem); // 장착된 아이템을 교체합니다
                Console.WriteLine($"\n{equippedItem.Name} 장착해제함."); // 교체하고 난 뒤 해당 문구를 출력합니다.
            }
        }

        if (!EquippedItems.Contains(itemToEquip)) // itemToEquip 값에 아무것도 장착이 안되어 있을 경우
        {
            EquippedItems.Add(itemToEquip); // itemToEquip 에 맞는 슬룻으로 장착합니다.
            Console.WriteLine($"\n{itemToEquip.Name} 장착함."); // 장착하고 난 뒤 "아이템이름 + 장착함" 문구를 출력합니다.
        }
    }
    public void ShowShop() // ShowShop 메서드가 실행될경우 {} 코드를 실행합니다.
    {
        bool shop = true; // shop 의 bool 값은 true 입니다.

        List<Item> itemsForSale = new List<Item>() // itemsForSale List<Item> 에 new List<Item>() 값을 보관합니다.
        {
            new Armor("무쇠갑옷      방어력 +5      무쇠로 만들어져 튼튼한 갑옷입니다.", 5, 50, ItemSlot.Armor), // 해당 코드는 new List<Item>() 값에 저장됩니다. 
            new Weapon("스파르타의 창      공격력 +7       스파트라의 전사들이 사용했다는 전설의 창 입니다.", 7, 50, ItemSlot.Weapon),
            new Weapon("낡은 검      공격력 +2      쉽게 볼 수 있는 낡은 검 입니다.", 2, 10, ItemSlot.Weapon),
            new Armor("철제갑옷      방어력 +10      철제로 만들어져 더욱 튼튼한 갑옷입니다.", 10, 100, ItemSlot.Armor),
            // 코드 순서대로 ("아이템 이름", 공격력 or 방어력, 가격, 아이템타입) 입니다. 공격력과 방어력 구분은 앞에 new Weapon 인지 new Armor 인지 에서 갈립니다.
            // new List<Item>() 으로 선언했는데 왜 new Armor or Weapon 인가요? 
            // 가장 아래에 내려보면 Armor : Item / Weapon : Item 으로 선언을 해둠으로써 Item 대신 사용할 수 있는 겁니다.
        };

        while (shop) // shop 이 true 일때
        {
            Console.WriteLine("\n== 상점 ==");
            for (float i = 0; i < itemsForSale.Count; i++) // itemsForSale 에 들어온 값을 0부터 가운트해서 i 변수에 추가합니다. i 는 숫자입니다.
            {
                Console.WriteLine($"{i + 1}. {itemsForSale[(int)i].Name} - {itemsForSale[(int)i].Price} G"); // "i 변수 +1 , 아이템 이름 - 아이템 가격 G" 문구를 출력합니다.
            }
            Console.WriteLine($"\n{itemsForSale.Count + 1}. 아이템 판매"); // itemsForSale +1 을 하여 아이템 의 가장 밑에 아이템 판매 문구를 출력합니다.
            Console.WriteLine("\n0. 나가기");

            Console.Write("\n원하시는 행동을 입력해주세요. >> ");
            string input = Console.ReadLine(); // 입력받은 값을 input에 문자열로 저장합니다.
            if (input == "0") // 0이면 shop false 합니다.
            {
                shop = false;
            }
            else if (float.TryParse(input, out float action) && action == itemsForSale.Count + 1) // 그게 아니라 itemsForSale +1 을 누르면
            {
                SellItem(); // 아이템 판매 메서드를 호출합니다.
            }
            else if (float.TryParse(input, out float selectedItemIndex) && selectedItemIndex > 0 && selectedItemIndex <= itemsForSale.Count) // i 변수 +1  을 선택했다면
            {
                BuyItem(selectedItemIndex - 1, itemsForSale); // 아이템을 구매합니다. (BuyItem 메서드 호출 / 실질적인 구매 절차는 해당 메서드에 있기 때문)
            }
            else // 이 위에 있는것들이 전부 아니라면
            {
                Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요."); // 문구를 출력합니다.
            }
        }
    }

    public void BuyItem(float itemIndex, List<Item> itemsForSale) // 아이템 구매 메서드 입니다.
    {
        Item selectedItem = itemsForSale[(int)itemIndex]; // 선택된 아이템을 itemsForSale 리스트에서 가져와서 selectedItem에 할당합니다.
        if (this.Gold >= selectedItem.Price) // 총 골드가 아이템 가격보다 많다면
        {
            this.Gold -= selectedItem.Price; // 아이템 가격만큼 총 골드에서 빼고
            this.Inventory.Add(selectedItem); // 아이템을 인벤토리에 추가합니다.
            Console.WriteLine($"\n{selectedItem.Name}을(를) 구매했습니다."); // 그리고 해당 문구를 출력합니다.
        }
        else // 그게 아니라면
        {
            Console.WriteLine("\n금액이 부족합니다."); // 문구를 출력합니다.
        }
    }

    public void SellItem() // 아이템 판매 메서드 입니다.
    {
        Console.WriteLine("\n== 인벤토리 ==");
        for (float i = 0; i < Inventory.Count; i++) // 선택된 'i 변수 +1' 값에 해당하는 값을 고른다면 즉, 아이템을 고른다면
        {
            Console.WriteLine($"\n{i + 1}. {Inventory[(int)i].Name} - 판매가: {(float)(Inventory[(int)i].Price * 0.85)} G"); // 아이템이름 - 판매가 : 원가의 85% G 를 출력합니다. (이거는 그냥 문구만 출력하는 코드, 실질적인 판매절차는 아래에 있음)
        }
        Console.WriteLine("\n0. 되돌아가기");

        Console.Write("\n판매할 아이템을 선택해주세요. >> ");
        string input = Console.ReadLine(); // 입력받은 값 input 에 문자열로 저장합니다.
        if (float.TryParse(input, out float itemIndex) && itemIndex > 0 && itemIndex <= Inventory.Count) // 만약 아이템을 고른다면
        {
            Item selectedItem = Inventory[(int)itemIndex - 1]; // 해당 아이템과 같은 값이 들어간 인벤토리의 아이템을 찾습니다.

            if (EquippedItems.Contains(selectedItem)) // 만약 장착중이라면
            {
                EquippedItems.Remove(selectedItem); // 장착을 해제시킵니다.
                Console.WriteLine($"\n{selectedItem.Name}을(를) 장착 해제하였습니다.");
            }

            this.Gold += (float)(selectedItem.Price * 0.85); // 총 골드량을 (아이템 원가의 85%) 만큼 추가합니다.
            Inventory.RemoveAt((int)itemIndex - 1); // 해당 아이템과 같은 값이 들어간 인벤토리의 아이템을 인벤토리에서 지웁니다.
            Console.WriteLine($"\n{selectedItem.Name}을(를) 판매했습니다.");
        }
        else if (input != "0") // 이상한거 입력하면
        {
            Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
        }
    }

    public void EnterDungeon() // 던전입장 메서드 입니다.
    {
        Console.WriteLine("\n던전에 입장합니다.");
        Console.WriteLine("\n1. 쉬움 (권장 방어력 : 5)");
        Console.WriteLine("2. 보통 (권장 방어력 : 10)");
        Console.WriteLine("3. 어려움 (권장 방어력 : 15)");
        Console.WriteLine("0. 돌아가기");

        Console.Write("\n원하시는 던전 난이도를 입력해주세요. >> ");
        string dungeonInput = Console.ReadLine(); // 입력받은 값을 dungeonInput 에 문자열로 저장합니다.

        float recommendedDefense = 0; // recommendedDefense 는 숫자이고, 기본값은 0 입니다. (권장 방어력)
        float baseReward = 0; // (던전의 기본 보상)

        switch (dungeonInput) // dungeonInput 는 다음과 같은 값을 입력받습니다.
        {
            case "1": // "1" 을 입력 받았을 때
                recommendedDefense = 5; // 권장 방어력 5 
                baseReward = 1000; // 던전의 기본보상 1000 골드
                break;
            case "2":
                recommendedDefense = 10;
                baseReward = 1700;
                break;
            case "3":
                recommendedDefense = 15;
                baseReward = 2500;
                break;
            case "0": // "0" 을 입력받으면 끕니다.
                return;
            default:
                Console.WriteLine("\n잘못된 입력입니다.");
                return;
        }

        AttemptDungeon(recommendedDefense, baseReward); // AttemptDungeon 메서드를 불러옵니다.
    }

    private void AttemptDungeon(float recommendedDefense, float baseReward) // AttemptDungeon 메서드 입니다. 해당 메서드는 던전의 클리어 여부, 보상을 얼마나 획득할지를 결정합니다.
    {
        Random rand = new Random(); // rand 에 new Random() 값을 받습니다.
        float totalDefense = CalculateTotalDefense(); // 총 방어력을 CalculateTotalDefense 메서드에서 가져와서 숫자로 저장합니다.
        float defenseDiff = totalDefense - recommendedDefense; // defenseDiff 값을 총방어력 - 권장 방어력 값으로 가져와서 숫자로 저장합니다.
        float healthLoss; // healthLoss 는 숫자입니다. 해당 변수는 체력이 얼마나 깎일지에 사용합니다.
        float finalReward = baseReward; // 최종보상은 baseReward의 값을 가져와서 숫자로 저장합니다.
        double bonus = ((Attack + (rand.Next((int)Attack, (int)Attack * 2) / 100.0)) * baseReward) / 10; // bonus 값은 공격력에 비례한 추가 보상 값을 가져와서 저장합니다.
        // int float double 의 차이
        // int = 정수 (정확함)
        // float = 소수포함
        // double = 소수포함
        // float 이랑 double 이랑 같은데요?
        // 컴퓨터는 숫자를 가져올때 2진수로 가져옵니다.
        // float 은 2진수를 가져올때 최대 32비트 까지 작성이 가능하고,
        // double 은 2진수를 가져올때 최대 64비트 까지 작성이 가능합니다.
        // C#의 Math 라이브러리 함수는 double 을 기본으로 사용합니다. 함수들과의 호환성을 위해 double 을 사용하였습니다.

        if (recommendedDefense > totalDefense) // 총 방어력이 권장 방어력보다 낮은경우
        {
            if (rand.Next(0, 100) < 40) // 확률 (랜덤) 을 돌립니다. 40% 확률로 실행
            {
                // 던전 실패
                Console.WriteLine("\n던전을 실패했습니다. 체력이 50 감소합니다.");
                Health -= 50; // 체력이 50 깎입니다.
                return; // 해당 메서드를 끕니다.
            }
            else // 40%가 안터지면
            {
                // 던전 성공하지만 보상 없음
                Console.WriteLine("\n던전을 클리어했지만, 보상을 받지 못했습니다. 체력이 50 깎입니다.");
                Health -= 50; // 체력 50 깎입니다.
                return; // 해당 메서드를 끕니다.
            }
        }
        else // 그게 아니라면 (권장 방어력이 현재 방어력보다 낮거나 같은 경우)
        {
            healthLoss = rand.Next(20 - (int)defenseDiff, 36 - (int)defenseDiff); // 깎일 체력을 방어력에 비례해서 랜덤으로 결정합니다.
            finalReward += (int)bonus; // 최종 보상에 보너스를 더합니다.
            Health -= healthLoss; // 체력을 위에서 계산한 값으로 깎습니다.
            Gold += finalReward; // 총 골드에 최종보상을 더합니다.
            DungeonClear(); // 던전 클리어 메서드를 호출합니다.
            Console.WriteLine($"\n던전을 성공적으로 클리어했습니다! \n체력이 {healthLoss} 감소하였습니다. \n보상으로 {finalReward}G를 획득했습니다.");
            Console.WriteLine("\n추가보상 : " + bonus + "G");
        }
    }

    private float CalculateTotalDefense() // 해당 메서드는 전체 방어력을 계산합니다.
    {
        float totalDefense = Defense; // totalDefense 의 기본값을 Defense 로 만듭니다.
        foreach (var item in EquippedItems) // 만약 장비를 끼고 있다면
        {
            if (item is Armor)
            {
                Armor armor = item as Armor;
                totalDefense += armor.DefenseBonus; // 해당 아이템 만큼 방어력을 더합니다.
            }
        }
        return totalDefense; // 계산된 totalDefense 값을 가지고 돌아갑니다.
    }

    public void Rest() // 휴식 메서드 입니다.
    {
        Console.WriteLine("500G를 지불하면 체력을 회복할 수 있습니다. (보유골드 : " + Gold + "G)");

        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("0. 나가기");

        Console.Write("\n원하시는 행동을 입력해주세요. >> ");
        string choice = Console.ReadLine(); // 값 입력 받습니다. 문자열 저장

        switch (choice)
        {
            case "1": // 1을 입력하면
                if (Gold >= 500) // 총 골드가 500보다 높을 경우
                {
                    Gold -= 500; // 500원을 깎고 
                    Health = 100; // 체력을 100으로 만듭니다.
                    Console.WriteLine("체력이 완전히 회복되었습니다. 남은 골드는 " + Gold + "G입니다.");
                }
                else // 아니면?
                {
                    Console.WriteLine("골드가 부족하여 휴식을 취할 수 없습니다."); // 돈없음
                }
                break;
            case "0":
                break;
            default:
                Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                break;
        }
    }

    private void DungeonClear() //
    {
        DungeonCleared++; // 던전 클리어 횟수를 증가
        CheckLevelUp(); // 레벨업 메서드를 불러옵니다.
    }

    private void CheckLevelUp() // 레벨업 메서드 입니다.
    {
        float requiredCleared = Level; // 현재 레벨에서 다음 레벨까지 필요한 클리어 횟수를 설정합니다. (레벨 1 = 1번, 레벨 2 = 2번)
        if (DungeonCleared >= requiredCleared) // 횟수 보다 던전을 많이 클리어 했다면,
        {
            Level++; // 레벨을 증가시킵니다.
            DungeonCleared = 0; // 다시 계산하기 위해 클리어 횟수를 초기화 합니다. 초기화 하지 않으면 총 2회 클리어를 했을때 바로 3렙이 되어버립니다. 
            Attack += 0.5f; // 공격력을 증가시킵니다.
            Defense += 1; // 방어력을 증가시킵니다.
            Console.WriteLine($"\n축하합니다! 레벨 {Level}로 상승했습니다.");
            Console.WriteLine($"공격력이 0.5 증가하여 현재 {Attack}입니다.");
            Console.WriteLine($"방어력이 1 증가하여 현재 {Defense}입니다.");
        }
    }

    [Serializable]
    public class Item // 아이템 클래스 입니다.
    {
        public string Name; // Name 은 문자열 입니다.
        public float Price; // Price 는 숫자 입니다.
        public ItemType Type; // Type 는 아이템 타입 입니다.

        public Item(string name, float price, ItemType type) // 아이템 (name, price, type) 을 저장합니다.
        {
            Name = name; // Name 문자열은 무엇을 의미하나요? 그건 아이템 내에 있는 name 으로 저장합니다.
            Price = price;
            Type = type;
        }

        public object Slot { get; internal set; } // Slot 값을 외부에서 접근 불가능하게 막고, 오브젝트에 저장합니다.
    }

    [Serializable]
    public class Weapon : Item // Weapon 클래스는 Item 클래스를 상속합니다.
    {
        public float AttackBonus; // 어택보너스를 숫자로 저장합니다.
        public ItemSlot Slot; // Slot 을 아이템슬룻에 저장합니다.

        public Weapon(string name, float attackBonus, float price, ItemSlot slot) : base(name, price, ItemType.Weapon) // Weapon (이름, 공격력, 가격, 아이템타입) 으로 저장합니다.
        {
            AttackBonus = attackBonus; // 어택보너스 숫자는 무엇을 의미하나요? 그건 Weapon 에 있는 attackBonus 으로 저장합니다.
            Slot = slot;
        }
    }

    [Serializable]
    public class Armor : Item // Weapon 과 같음 차이점 : 공격력 -> 방어력으로 바뀜
    {
        public float DefenseBonus;
        public ItemSlot Slot;

        public Armor(string name, float defenseBonus, float price, ItemSlot slot) : base(name, price, ItemType.Armor)
        {
            DefenseBonus = defenseBonus;
            Slot = slot;
        }
    }

    [Serializable]
    public enum ItemType // 아이템 타입의 종류입니다.
    {
        Weapon,
        Armor
    }

    [Serializable]
    public enum ItemSlot // 아이템 슬룻의 종류입니다.
    {
        Weapon,
        Armor
    }
}