using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight; // Ссылка на компонент света
    public float minIntensity = 0.5f; // Минимальная интенсивность света
    public float maxIntensity = 1.5f; // Максимальная интенсивность света
    public float minFlickerSpeed = 2.0f; // Минимальная скорость мерцания в секундах
    public float maxFlickerSpeed = 10.0f; // Максимальная скорость мерцания в секундах

    private void Start()
    {
        if (flickerLight == null)
        {
            // Если ссылка на свет не задана, пытаемся получить компонент Light у того же GameObject
            flickerLight = GetComponent<Light>();
        }
    }

    private void Update()
    {
        if (flickerLight != null)
        {
            // Рандомизация интенсивности света в заданных пределах
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // Переключаем включение и выключение света
            flickerLight.enabled = !flickerLight.enabled;

            // Рандомизация времени ожидания перед следующим переключением
            float randomFlickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
            yield return new WaitForSeconds(randomFlickerSpeed);
        }
    }

    private void OnEnable()
    {
        // Запускаем корутину мерцания света
        StartCoroutine(Flicker());
    }

    private void OnDisable()
    {
        // Останавливаем корутину, если объект деактивирован
        StopCoroutine(Flicker());
    }
}
