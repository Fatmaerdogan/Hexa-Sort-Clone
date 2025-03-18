using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HexagonStack : MonoBehaviour
{
    public List<Hexagon> Hexagons { get; private set; }
    public void Add(Hexagon hexagon)
    {
        if (Hexagons == null)
            Hexagons = new List<Hexagon>();

        Hexagons.Add(hexagon);
        hexagon.SetParent(transform);
    }
    public Material GetTopHexagonMaterial()
    {
        return Hexagons[^1].MaterialSet;
    }

    public void Place()
    {
        foreach (Hexagon hexagon in Hexagons)
            hexagon.DisableCollider();
    }
    public bool Contains(Hexagon hexagon) => Hexagons.Contains(hexagon);

    public void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);
        if (Hexagons.Count <= 0)
            DestroyImmediate(gameObject);
    }

}
