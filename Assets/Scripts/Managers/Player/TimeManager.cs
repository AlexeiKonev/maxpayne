using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private PostProcessVolume mainPPVolume;
    [SerializeField] private PostProcessProfile mainPPProfile;
    [SerializeField] private PostProcessProfile slowtimePPProfile;
    [SerializeField] private float slowedTime = 0.5f;

    public void ToggleSlowMotion()
    {
        if (Time.timeScale == slowedTime)
        {
            NormalizeTime();
        }
        else
        {
            SlowDownTime();
        }
    }

    private void SlowDownTime()
    {
        mainPPVolume.profile = slowtimePPProfile;
        Time.timeScale = slowedTime;
    }

    private void NormalizeTime()
    {
        mainPPVolume.profile = mainPPProfile;
        Time.timeScale = 1f;
    }
}
//Этот класс отвечает за управление замедлением времени, используя PostProcessVolume и Time.timeScale.

//Теперь у вас есть пять отдельных скриптов, каждый из которых отвечает за свою специфическую функцию. Не забудьте добавить все необходимые компоненты и ссылки в инспекторе Unity для каждого GameObject. Эта структура улучшает читаемость, поддерживаемость и позволяет легче расширять функциональность проекта. Если у вас есть еще вопросы или нужна помощь, не стесняйтесь задавать.