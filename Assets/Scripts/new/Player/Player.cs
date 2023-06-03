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

    public List<GameObject> unlockedWeapons; // ������ �������� ������
    public GameObject[] allWeapons; // ��� ������
    public Image weaponIcon; // ������ ������

    public FishBooster ribovTimer; // ������ ����
    public GameObject booster; // ������

    private Gravity gravity; // ����������

    public Animator animator; // ��������

    private int idScene;

    [SerializeField] private PlayerSettings playerSettings; // ��������� ��������� (�� ������ ������ ������ ��������)

    private IPlayerInput playerInput;  // ��������� �����������
    private PlayerMove playerMove; // ���������� ���������

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // ��������� ����������� ������ ��� ����������
        RigidBody = GetComponent<Rigidbody>(); // ��������� rigid body
        RigidBody.AddForce(0, 0, 2.0f, ForceMode.Impulse); // ���������� ��������
        gravity = GetComponent<Gravity>();
        vectorToSafe = this.transform.position;
        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        
        playerInput = new ControllerInput(); // �������� ���������� �����

        playerMove = new PlayerMove(playerInput, vector, playerSettings, animator); // �������� �������� ������ ������� � ����� ���������
    }


    private void Update()
    {
        displayHealth(); // ����������� �������� �� ������
        Move(); // ��������
        vectorToSafe = this.transform.position;
        idScene = SceneManager.GetActiveScene().buildIndex;
        gravity.Gravitation(); //����������

        if (Input.GetKeyDown(KeyCode.Q)) // ���� ������ q �� ������� ������
        {
            SwitchWeapon(); // ����� ������
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Power Up")) // ���� �������� ��� power up 
        {
            FoundPowerUpTimer();
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
        playerMove.Tick(characterController, gravity); // ��������� �������� �� �����������

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
        healthDisplay.text = "HP: " + health; // ���������� �� �� ������
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
        ChangeHealth(5);
        Destroy(other.gameObject);
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
        FishBooster[] obj = Resources.FindObjectsOfTypeAll<FishBooster>();
        foreach (var item in obj)
        {
            if (item.name == "Power Up Timer")
            {
                ribovTimer = item;
            }
        }
    }

    public void LoadData(DataGame data)
    {
        //data.currentHpPlayer
        //data.currentPlacePlayer
        //data.isFlippedRighth

        this.health = data.currentHpPlayer;
        this.flipRight = data.isFlippedRight;
        vectorToSafe = data.currentPlacePlayer;
        idScene = data.SceneNumber;
    }

    public void SaveData(ref DataGame data)
    {
        data.currentHpPlayer = (int) this.health;
        data.isFlippedRight = flipRight;
        data.currentPlacePlayer = vectorToSafe;
        data.SceneNumber = SceneManager.GetActiveScene().buildIndex;

    }
}



