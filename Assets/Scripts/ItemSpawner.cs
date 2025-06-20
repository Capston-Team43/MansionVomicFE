using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    [Header("������ �����հ� ���� ���� (���� �߿�)")]
    public GameObject[] itemPrefabs;  // [�⸧��, ����, ������, ����, ����, ��ġ, ����, ����, ����, ��, ������]
    public int[] itemCounts = { 2, 5, 1, 1, 2, 1, 1, 3, 1, 1, 1 };

    [Header("ū ������ �ε��� (itemPrefabs �迭 ����)")]
    public List<int> largeItemIndices = new List<int> { 0, 4, 5, 6, 8, 9, 10 }; // �⺻��: �⸧��~��

    [Header("���� ���� ������ ����")]
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

        //Debug.Log($"[ItemSpawner] ���� ������ {oldItems.Length}�� ����");
    }


    void SpawnItems()
    {
        // ������ Ȯ��
        List<int> finalItemList = new List<int>();
        for (int i = 0; i < itemCounts.Length; i++)
        {
            if (i == itemCounts.Length - 1 && Random.value > 0.9f) continue; // ������ index = 10
            for (int j = 0; j < itemCounts[i]; j++)
            {
                finalItemList.Add(i); // itemPrefabs[i]
            }
        }

        finalItemList = finalItemList.OrderBy(x => Random.value).ToList();

        string[] floorNames = { "B1", "Floor1", "Floor2", "Floor3", "Attic" };

        Dictionary<string, List<GameObject>> roomsByFloor = new Dictionary<string, List<GameObject>>();
        Dictionary<string, int> floorItemLimit = new Dictionary<string, int>();

        // �ʱ�ȭ
        foreach (string floor in floorNames)
        {
            roomsByFloor[floor] = new List<GameObject>();
            floorItemLimit[floor] = Random.Range(minItemsPerFloor, maxItemsPerFloor + 1);
        }

        // �� ã��
        GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in allRooms)
        {
            Transform parent = room.transform.parent;
            if (parent != null && roomsByFloor.ContainsKey(parent.name))
            {
                roomsByFloor[parent.name].Add(room);
            }
        }

        // ������ ����
        foreach (int itemIndex in finalItemList)
        {
            // ���� ������ �ִ� ��
            List<string> availableFloors = floorItemLimit.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();
            if (availableFloors.Count == 0) break;

            // ���� �� ����
            string selectedFloor = availableFloors[Random.Range(0, availableFloors.Count)];
            List<GameObject> floorRooms = roomsByFloor[selectedFloor];
            if (floorRooms.Count == 0) continue;

            GameObject selectedRoom = floorRooms[Random.Range(0, floorRooms.Count)];

            // ������ ũ�� �з�
            bool isLarge = largeItemIndices.Contains(itemIndex);

            // ���� ���� ��ġ ��ȯ
            Transform spawnPoint = GetSpawnPointInRoom(selectedRoom.transform, isLarge);

            // ������ ����
            Quaternion spawnRotation = spawnPoint.rotation;
            GameObject spawned = Instantiate(itemPrefabs[itemIndex], spawnPoint.position, spawnRotation);
            spawned.transform.SetParent(spawnPoint.parent); // �ִϸ��̼� Ÿ���� ������ ����

            // ������~
            string itemName = itemPrefabs[itemIndex].name;
            string roomName = selectedRoom.name;
            string furnitureName = spawnPoint.parent != null ? spawnPoint.parent.name : "RoomCenter";

            // Debug.Log($"[Item Spawned] \"{itemName}\" ������ �� ��: {selectedFloor}, ��: {roomName}, ��ġ: {furnitureName}");

            floorItemLimit[selectedFloor]--;
        }
    }

    Transform GetSpawnPointInRoom(Transform room, bool isLarge)
    {
        string targetTag = isLarge ? "BigFurniture" : "SmallFurniture";

        // room �� �ڽ� �� �±װ� ��ġ�ϴ� ������Ʈ ã��
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

            // SpawnPoint ã��
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
                Debug.LogWarning($"SpawnPoint �±װ� {chosenFurniture.name} �ȿ� �����ϴ�. �ش� ���� ��ġ ���.");
                return chosenFurniture;
            }
        }
        else
        {
            return room; // ���� ������ �� �߽� ���
        }
    }

}
