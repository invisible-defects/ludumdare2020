using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameUI : MonoBehaviour
{
    [SerializeField]
    private float spawnDist = 1f;

    [SerializeField]
    private GameObject spikeSprite;
    [SerializeField]
    private int spikePixelWidth;

    [SerializeField]
    private float unitPerPixel = 1f;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private Transform spikeParent;

    [SerializeField]
    private float minDist = 1f;

    private List <RectTransform> spikes = new List<RectTransform>();

    [SerializeField]
    private RectTransform frame;

    [SerializeField]
    private Vector2 sizeDelta = new Vector2(20, 20);

    private Vector2 baseSize;

    private float battery = 1f;

    [SerializeField]
    private float batterySpeed = 0.2f;
    [SerializeField]
    private float batteryPenalty = 0.3f;
    [SerializeField]
    private float batteryAdd = 0.2f;

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Image batteryImg;

    private void Start()
    {
        baseSize = frame.sizeDelta;
        GameManager.Instance.state.OnChanged += this.StateOnChanged;
    }

    private void Update()
    {
        if (GameManager.Instance.state.Value != GameManager.State.Playing)
            return;

        foreach (var spike in spikes)
        {
            spike.anchoredPosition -= Vector2.right * speed;
        }

        while (spikes.Count > 0 && spikes[0].anchoredPosition.x < -spawnDist)
        {
            var spike = spikes[0];
            spikes.RemoveAt(0);
            Destroy(spike.gameObject);
        }

        var distance = spikes.Count > 0 ?
            spawnDist - spikes[spikes.Count - 1].anchoredPosition.x :
            Mathf.Infinity;

        if (distance > minDist)
        {
            var spike = Instantiate(spikeSprite, spikeParent);
            var rt = spike.GetComponent<RectTransform>();
            rt.anchoredPosition3D = new Vector3(spawnDist, 0, 1);
            spikes.Add(rt);
        }

        var underCenter = CheckSpike();

        if (underCenter == null)
        {
            frame.sizeDelta = baseSize;
        }
        else
        {
            frame.sizeDelta = baseSize + sizeDelta;
        }

        battery -= batterySpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (underCenter == null)
            {
                battery -= batteryPenalty;
            }
            else
            {
                spikes.Remove(underCenter);
                Destroy(underCenter.gameObject);
                battery += batteryAdd;
            }
        }

        battery = Mathf.Clamp01(battery);

        batteryImg.fillAmount = battery;

        if (Mathf.Approximately(battery, 0))
        {
            player.Death();
        }
    }

    private RectTransform CheckSpike()
    {
        foreach (var spike in spikes)
        {
            var extent = spikePixelWidth * unitPerPixel * 0.5f;
            var left = spike.anchoredPosition.x - extent;
            var right = spike.anchoredPosition.x + extent;

            if (left <= 0 && 0 <= right)
            {
                return spike;
            }
        }

        return null;
    }

    private void StateOnChanged()
    {
        if (GameManager.Instance.state.Value == GameManager.State.MainMenu)
        {
            //Reset
            foreach (var spike in spikes)
            {
                Destroy(spike.gameObject);
            }

            spikes.Clear();

            battery = 1f;
        }
    }
}
