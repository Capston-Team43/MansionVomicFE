using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    [Header("아이템 프리팹과 개수 설정 (순서 중요)")]
    public GameObject[] itemPrefabs;  // [기름통, 종이, 라이터, 성냥, 물통, 망치, 도끼, 밧줄, 말뚝, 삽, 마법봉]
    public int[] itemCounts = { 2, 5, 1, 1, 2, 1, 1, 3, 1, 1, 1 };

    [Header("큰 아이템 인덱스 (itemPrefabs 배열 기준)")]
    public List<int> largeItemIndices = new List<int> { 0, 4, 5, 6, 8, 9, 10 }; // 기본값: 기름통~삽

    [Header("층당 생성 아이템 개수")]
    public int minItemsPerFloor = 2;
    public int maxItemsPerFloor = 6;

    void Start()
    {
        ClearOldItems();
        SpawnItems();
    }

    void ClearOldItems()
    {
        GameObject[] oldItems = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in oldItems)
        {
            Destroy(item);
        }

        //Debug.Log($"[ItemSpawner] 기존 아이템 {oldItems.Length}개 삭제");
    }


    void SpawnItems()
    {
        // 마법봉 확률
        List<int> finalItemList = new List<int>();
        for (int i = 0; i < itemCounts.Length; i++)
        {
            if (i == itemCounts.Length - 1 && Random.value > 0.9f) continue; // 마법봉 index = 10
            for (int j = 0; j < itemCounts[i]; j++)
            {
                finalItemList.Add(i); // itemPrefabs[i]
            }
        }

        finalItemList = finalItemList.OrderBy(x => Random.value).ToList();

        string[] floorNames = { "B1", "Floor1", "Floor2", "Floor3", "Attic" };

        Dictionary<string, List<GameObject>> roomsByFloor = new Dictionary<string, List<GameObject>>();
        Dictionary<string, int> floorItemLimit = new Dictionary<string, int>();

        // 초기화
        foreach (string floor in floorNames)
        {
            roomsByFloor[floor] = new List<GameObject>();
            floorItemLimit[floor] = Random.Range(minItemsPerFloor, maxItemsPerFloor + 1);
        }

        // 방 찾기
        GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in allRooms)
        {
            Transform parent = room.transform.parent;
            if (parent != null && roomsByFloor.ContainsKey(parent.name))
            {
                roomsByFloor[parent.name].Add(room);
            }
        }

        // 아이템 스폰
        foreach (int itemIndex in finalItemList)
        {
            // 남은 공간이 있는 층
            List<string> availableFloors = floorItemLimit.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();
            if (availableFloors.Count == 0) break;

            // 랜덤 층 선택
            string selectedFloor = availableFloors[Random.Range(0, availableFloors.Count)];
            List<GameObject> floorRooms = roomsByFloor[selectedFloor];
            if (floorRooms.Count == 0) continue;

            GameObject selectedRoom = floorRooms[Random.Range(0, floorRooms.Count)];

            // 아이템 크기 분류
            bool isLarge = largeItemIndices.Contains(itemIndex);

            // 가구 안쪽 위치 반환
            Transform spawnPoint = GetSpawnPointInRoom(selectedRoom.transform, isLarge);

            // 아이템 생성
            Quaternion spawnRotation = spawnPoint.rotation;
            GameObject spawned = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnRotation);
            spawned.transform.SetParent(spawnPoint.parent); // 애니메이션 타겟인 서랍에 붙임

            // 디버깅용~
            string itemName = itemPrefabs[itemIndex].name;
            string roomName = selectedRoom.name;
            string furnitureName = spawnPoint.parent != null ? spawnPoint.parent.name : "RoomCenter";

            // Debug.Log($"[Item Spawned] \"{itemName}\" 생성됨 → 층: {selectedFloor}, 방: {roomName}, 위치: {furnitureName}");

            floorItemLimit[selectedFloor]--;
        }
    }

    Transform GetSpawnPointInRoom(Transform room, bool isLarge)
    {
        string targetTag = isLarge ? "BigFurniture" : "SmallFurniture";

        // room 안 자식 중 태그가 일치하는 오브젝트 찾기
        Transform[] allChildren = room.GetComponentsInChildren<Transform>();
        List<Transform> furnitureList = new List<Transform>();

        foreach (Transform child in allChildren)
        {
            if (child.CompareTag(targetTag))
                furnitureList.Add(child);
        }

        if (furnitureList.Count > 0 && Random.value > 0.0f)
        {
            Transform chosenFurniture = furnitureList[Random.Range(0, furnitureList.Count)];

            // SpawnPoint 찾기
            Transform[] innerChildren = chosenFurniture.GetComponentsInChildren<Transform>();
            List<Transform> spawnPoints = new List<Transform>();

            foreach (Transform child in innerChildren)
            {
                if (child.CompareTag("SpawnPoint"))
                    spawnPoints.Add(child);
            }

            if (spawnPoints.Count > 0)
            {
                return spawnPoints[Random.Range(0, spawnPoints.Count)];
            }
            else
            {
                Debug.LogWarning($"SpawnPoint 태그가 {chosenFurniture.name} 안에 없습니다. 해당 가구 위치 사용.");
                return chosenFurniture;
            }
        }
        else
        {
            return room; // 가구 없으면 방 중심 사용
        }
    }

}
