using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character , IDataPersist
{

    public bool flipRight = true; // �������� �� �����?
    private Vector3 vector; // �����������
    private Vector3 vectorToSafe;
    public Rigidbody RigidBody; // ����� ����

    public Text healthDisplay; // ����������� �������� �� ������
    public float health; // ��������

    //public CharacterController characterController;

    public List<GameObject> unlockedWeapons; // ������ �������� ������
    public GameObject[] allWeapons; // ��� ������
    public Image weaponIcon; // ������ ������

    public FishBooster ribovTimer; // ������ ����
    public GameObject booster; // ������

    private Gravity gravity; // ����������

    public Animator animator; // ��������

    [SerializeField] private PlayerSettings playerSettings; // ��������� ��������� (�� ������ ������ ������ ��������)

    private IPlayerInput playerInput;  // ��������� �����������
    private PlayerMove playerMove; // ���������� ���������

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // ��������� ����������� ������ ��� ����������
        RigidBody = GetComponent<Rigidbody>(); // ��������� rigid body
        RigidBody.AddForce(0, 0, 2.0f, ForceMode.Impulse); // ���������� ��������
        gravity = GetComponent<Gravity>();
        vectorToSafe = transform.position;
        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        playerInput = new ControllerInput(); // �������� ���������� �����

        playerMove = new PlayerMove(playerInput, transform.position, playerSettings, animator); // �������� �������� ������ ������� � ����� ���������
        //DontDestroyOnLoad(this.gameObject);
    }


    private void Update()
    {
        displayHealth(); // ����������� �������� �� ������
        Move(); // ��������
        //vectorToSafe = this.transform.position;
        gravity.Gravitation(); //����������

        FoundPowerUpTimer();

        FoundImageOnCanvas();

        if (Input.GetKeyDown(KeyCode.Q)) // ���� ������ q �� ������� ������
        {
            SwitchWeapon(); // ����� ������
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Power Up")) // ���� �������� ��� power up 
        {
            boosters(other); // ��������� �������

        }
        else if (other.CompareTag("Weapon")) // ���� �������� ���� Weapon ��
        {
            weapon(other); // ������� ������ � �������� � ������ ���������
            
        } else
        if (other.CompareTag("Potion"))
        {
            potion(other); // �������� ���������� ��
        }
    }

    public void SwitchWeapon()
    {
            for (int i = 0; i < unlockedWeapons.Count; i++) // ���� �� ���������������� �������
            {
                if (unlockedWeapons[i].activeInHierarchy) // ���� ���������������� ������ ������� � �������� ��
                {
                    unlockedWeapons[i].SetActive(false); // ������� ������ ����������
                    if (i != 0) // ���� ������ �� ������ � ������ �� 
                    {
                        unlockedWeapons[i - 1].SetActive(true); // ������� ��������
                        weaponIcon = GameObject.FindGameObjectWithTag("Weapon Image").GetComponent<Image>();
                        weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite; // ���������� �� ������ ������ ������ �������� �� ������ ������
                    }
                    else
                    {
                        unlockedWeapons[unlockedWeapons.Count - 1].SetActive(true);
                        weaponIcon.sprite = unlockedWeapons[unlockedWeapons.Count - 1].GetComponent<SpriteRenderer>().sprite;   
                    }
                    weaponIcon.SetNativeSize();
                    break;
                }
            }
    }

    protected override void Move() 
    {
        playerInput.ReadInput(); // ������� �����������
        Debug.LogError("MOVE2 " + transform.position.z);
        playerMove.Tick(characterController, gravity); // ��������� �������� �� �����������
        Debug.LogError("MOVE3 " + transform.position.z);
        animaion(); // ��������� ��������

        fliped(); // ��������� ���������, ���� �������� ����������� ��������

    }

    protected override void Attack()
    {
        base.Attack(); // ��������� ������ ����� �� ������ ��������
    }

    public void ChangeHealth(int healthValue)
    {
        health += healthValue; // �������� ��
        displayHealth(); // ���������� �� �� ������
    }


    public void displayHealth()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = "HP: " + health; // ���������� �� �� ������
        }
        else
        {
            healthDisplay = GameObject.FindGameObjectWithTag("Health").GetComponent<Text>();
        }
    }


    public void boosters(Collider other)
    {
        booster.SetActive(true); // ������� ������ ��������
        ribovTimer.gameObject.SetActive(true); // �������� ������ ������� �����
        if(ribovTimer.gameObject != null)
        {
            Destroy(other.gameObject); // ��� ��������������� � �������� ����� �� ������������
        }
    }

    public void weapon(Collider other)
    {
        for (int i = 0; i < allWeapons.Length; i++)
        {
            if (other.name == allWeapons[i].name)
            {
                unlockedWeapons.Add(allWeapons[i]); // �������� ������ � ���������������� ������ ������
            }
        }
        // SwitchWeapon(); // ��� �������� ������������� ����������� ������
        Destroy(other.gameObject); // ��������� �������� ������
    }

    public void potion(Collider other)
    {
        ChangeHealth(5); // �������� 5 ��
        Destroy(other.gameObject); // ��������� ������
    }

    public void animaion()
    {
        if (playerMove.vector.x == -playerInput.Horizontal && playerMove.vector.z == -playerInput.Verical)
        {
            animator.SetBool("Walking", true); // ��������� �������� ��������
        }
        else
            animator.SetBool("Walking", false); // ���������� ��������
    }

    public void fliped()
    {
        if (facingRight && playerMove.vector.x > 0) // ���� ���� �� ���� �� ���������
        {
            Flip();
        }
        else if (!facingRight && playerMove.vector.x < 0) // ���� ���� �� ����� �� ���������
        {
            Flip();
        }
    }

    public void FoundPowerUpTimer()
    {
        if (ribovTimer == null)
        {
            FishBooster[] obj = Resources.FindObjectsOfTypeAll<FishBooster>(); // ����� � �������� ������� � ����� FishBooster
            foreach (var item in obj)
            {
                if (item.name.Equals("Power Up Timer")) // ���� � ������� �������� �������
                {
                    ribovTimer = item; // ��������� ������� �������
                }
            }
        }
    }

    public void FoundImageOnCanvas()
    {
        if(weaponIcon == null)
        {
            weaponIcon = GameObject.FindGameObjectWithTag("Weapon Image").GetComponent<Image>();
        }
    }

    public void LoadData(DataGame data)
    {
        this.health = data.currentHpPlayer; // �������� �������� �� ����������
        this.flipRight = data.isFlippedRight; // ��������� ������ �� ����������

        characterController.enabled = false;
        vectorToSafe = data.currentPlacePlayer; // ��������� ������� ���������� �� ����������
        vector = data.currentPlacePlayer;
        transform.position = data.currentPlacePlayer; // ��������� ������� ���������� �� ����������
        characterController.enabled = true;
    }

    public void SaveData(ref DataGame data)
    {
        data.currentHpPlayer = (int) this.health; // ��������� ��������
        data.isFlippedRight = flipRight; // ��������� ������
        data.currentPlacePlayer = vectorToSafe; // ��������� ������� ����������
    }
}



