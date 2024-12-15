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
//���� ����� �������� �� ���������� ����������� �������, ��������� PostProcessVolume � Time.timeScale.

//������ � ��� ���� ���� ��������� ��������, ������ �� ������� �������� �� ���� ������������� �������. �� �������� �������� ��� ����������� ���������� � ������ � ���������� Unity ��� ������� GameObject. ��� ��������� �������� ����������, ���������������� � ��������� ����� ��������� ���������������� �������. ���� � ��� ���� ��� ������� ��� ����� ������, �� ����������� ��������.