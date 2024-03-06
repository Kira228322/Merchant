using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mover : MonoBehaviour
{
    // ���������� ����� ����� ������� ������� � ������� RandomBGGenerator, ��, ��� ������ ������ Mover �� �������� �� ������
    // ��� ����������, ������� ����� ������... �� ����� � ���� ���-�� � ������ ������������ ��������, ���� ���
    [SerializeField] private RawImage NearBG;
    [SerializeField] private RawImage FarBG;
    [SerializeField] private float _speed;
    public float Speed => _speed;
    [SerializeField] private Tilemap ground1;
    [SerializeField] private Tilemap ground2;
    private int n = 1;

    private float RawImageChange;
    void Update()
    {
        transform.position += new Vector3(_speed * Time.deltaTime, 0);

        RawImageChange += _speed * Time.deltaTime / 125;
        NearBG.uvRect = new Rect(RawImageChange, 0, NearBG.uvRect.width, NearBG.uvRect.height);
        FarBG.uvRect = new Rect(RawImageChange / 4, 0, FarBG.uvRect.width, FarBG.uvRect.height);
    }

    private void FixedUpdate() // ����� ��������� (������ ��� �� ����� ���������) 
    {
        if (transform.position.x >= 26 * n)
        {
            if (n % 2 == 1)
            {
                ground1.transform.position += new Vector3(52, 0, 0);
            }
            else
            {
                ground2.transform.position += new Vector3(52, 0, 0);
            }
            n++;
        }
    }
}
