using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Subsystems;

public class StackSpawner : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexagonStack hexagonStackPrefab;

    [Header("Settings")]
    [NaughtyAttributes.MinMaxSlider(2, 8)]
    [SerializeField] private Vector2Int minMaxHexCount;
    [SerializeField] private Material[] CharacterMaterials;

    private int stackCounter;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void OnEnable()
    {
        StackController.onStackPlaced += StackPlacedCallback;
    }
  
    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }
    private void StackPlacedCallback(GridCell gridCell)
    {
        stackCounter++;

        if (stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }


    void Start()
    {
        GenerateStacks();
    }

    private void GenerateStacks()
    {
        for (int i = 0; i < stackPositionsParent.childCount; i++)
        {
            GenerateStack(stackPositionsParent.GetChild(i));
        }
    }

    private void GenerateStack(Transform parent)
    {
        HexagonStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex()}";

        int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);

        int firstColorHexagonCount = Random.Range(0, amount);

        Material[] materialsArray = GetRandomMaterials();
        for (int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .2f;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);

            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, hexStack.transform);
            hexagonInstance.MaterialSet =  i < firstColorHexagonCount ? materialsArray[0] : materialsArray[1];

            hexagonInstance.Configure(hexStack);
            hexStack.Add(hexagonInstance);
        }
    }

    private Material[] GetRandomMaterials()
    {
        List<Material> MaterialList = new List<Material>();
        MaterialList.AddRange(CharacterMaterials);

        if (MaterialList.Count <= 0)
        {
            Debug.LogError("No color found");
            return null;
        }

        Material firstMaterial = MaterialList.OrderBy(x => Random.value).First();
        MaterialList.Remove(firstMaterial);

        if (MaterialList.Count <= 0)
        {
            Debug.LogError("Only one color was found");
            return null;
        }

        Material secondMaterial = MaterialList.OrderBy(x => Random.value).First();

        return new Material[] { firstMaterial, secondMaterial };
    }

}
