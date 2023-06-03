using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character , IDataPersist
{

    public bool flipRight = true; // повернут на право?
    private Vector3 vector; // направление
    private Vector3 vectorToSafe;
    public Rigidbody RigidBody; // ригид боди

    public Text healthDisplay; // отображение здоровья на экране
    public float health; // здоровье

    public List<GameObject> unlockedWeapons; // список открытых оружий
    public GameObject[] allWeapons; // все оружия
    public Image weaponIcon; // иконка оружия

    public FishBooster ribovTimer; // бустер рыба
    public GameObject booster; // бустер

    private Gravity gravity; // гравитация

    public Animator animator; // аниматор

    private int idScene;

    [SerializeField] private PlayerSettings playerSettings; // Настройки персонажа (На данный момент только скорость)

    private IPlayerInput playerInput;  // Интерфейс направления
    private PlayerMove playerMove; // Управление движением

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // получение контроллера игрока для управления
        RigidBody = GetComponent<Rigidbody>(); // получение rigid body
        RigidBody.AddForce(0, 0, 2.0f, ForceMode.Impulse); // добавление движения
        gravity = GetComponent<Gravity>();
        vectorToSafe = this.transform.position;
        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        
        playerInput = new ControllerInput(); // Получить контроллер ввода

        playerMove = new PlayerMove(playerInput, vector, playerSettings, animator); // Получить движение игрока внедряя в класс параметры
    }


    private void Update()
    {
        displayHealth(); // отображение здоровья на экране
        Move(); // Движение
        vectorToSafe = this.transform.position;
        idScene = SceneManager.GetActiveScene().buildIndex;
        gravity.Gravitation(); //Гравитация

        if (Input.GetKeyDown(KeyCode.Q)) // если нажата q то вменить оружие
        {
            SwitchWeapon(); // смена оружия
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Power Up")) // если встречен тэг power up 
        {
            FoundPowerUpTimer();
            boosters(other); // включение бустера

        }
        else if (other.CompareTag("Weapon")) // если встречен теэг Weapon то
        {
            weapon(other); // поднять оружие и добавить в список доступных
            
        } else
        if (other.CompareTag("Potion"))
        {
            potion(other); // Добавить количество ХП
        }
    }

    public void SwitchWeapon()
    {
            for (int i = 0; i < unlockedWeapons.Count; i++) // цикл по разблокированным оружиям
            {
                if (unlockedWeapons[i].activeInHierarchy) // есои разблокированное оружие активно в иерархии то
                {
                    unlockedWeapons[i].SetActive(false); // сделать оружие неактивным
                    if (i != 0) // если оружие не первое в списке то 
                    {
                        unlockedWeapons[i - 1].SetActive(true); // сделать активным
                        weaponIcon = GameObject.FindGameObjectWithTag("Weapon Image").GetComponent<Image>();
                        weaponIcon.sprite = unlockedWeapons[i - 1].GetComponent<SpriteRenderer>().sprite; // отобразить на экране иконку оружия надетого на данный момент
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
        playerInput.ReadInput(); // Считать направление
        playerMove.Tick(characterController, gravity); // выполнить движение по направлению

        animaion(); // включение анимации

        fliped(); // повернуть персонажа, если смнилось направление движения

    }

    protected override void Attack()
    {
        base.Attack(); // выполнить метода атаки из класса родителя
    }

    public void ChangeHealth(int healthValue)
    {
        health += healthValue; // добавить ХП
        displayHealth(); // отобразить ХП на экране
    }


    public void displayHealth()
    {
        healthDisplay.text = "HP: " + health; // отобразить ХП на экране
    }


    public void boosters(Collider other)
    {
        booster.SetActive(true); // Сделать бустер активным
        ribovTimer.gameObject.SetActive(true); // включить таймер бустера рыбки
        if(ribovTimer.gameObject != null)
        {
            Destroy(other.gameObject); // При соприкосновении с объектом повер ап уничтожается
        }
    }

    public void weapon(Collider other)
    {
        for (int i = 0; i < allWeapons.Length; i++)
        {
            if (other.name == allWeapons[i].name)
            {
                unlockedWeapons.Add(allWeapons[i]); // добавить оружие в разблокированный список оружий
            }
        }
        // SwitchWeapon(); // при поднятии автоматически переключить оружие
        Destroy(other.gameObject); // разрушить поднятый объект
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
            animator.SetBool("Walking", true); // выполнить анимацию хождения
        }
        else
            animator.SetBool("Walking", false); // остановить анимацию
    }

    public void fliped()
    {
        if (facingRight && playerMove.vector.x > 0) // если идет на лево то повернуть
        {
            Flip();
        }
        else if (!facingRight && playerMove.vector.x < 0) // если идет на право то повернуть
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



