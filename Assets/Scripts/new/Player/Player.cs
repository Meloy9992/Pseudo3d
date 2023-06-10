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

    //public CharacterController characterController;

    public List<GameObject> unlockedWeapons; // список открытых оружий
    public GameObject[] allWeapons; // все оружия
    public Image weaponIcon; // иконка оружия

    public FishBooster ribovTimer; // бустер рыба
    public GameObject booster; // бустер

    private Gravity gravity; // гравитация

    public Animator animator; // аниматор

    [SerializeField] private PlayerSettings playerSettings; // Настройки персонажа (На данный момент только скорость)

    private IPlayerInput playerInput;  // Интерфейс направления
    private PlayerMove playerMove; // Управление движением

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // получение контроллера игрока для управления
        RigidBody = GetComponent<Rigidbody>(); // получение rigid body
        RigidBody.AddForce(0, 0, 2.0f, ForceMode.Impulse); // добавление движения
        gravity = GetComponent<Gravity>();
        vectorToSafe = transform.position;
        //GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        playerInput = new ControllerInput(); // Получить контроллер ввода

        playerMove = new PlayerMove(playerInput, transform.position, playerSettings, animator); // Получить движение игрока внедряя в класс параметры
        //DontDestroyOnLoad(this.gameObject);
    }


    private void Update()
    {
        displayHealth(); // отображение здоровья на экране
        Move(); // Движение
        //vectorToSafe = this.transform.position;
        gravity.Gravitation(); //Гравитация

        FoundPowerUpTimer();

        FoundImageOnCanvas();

        if (Input.GetKeyDown(KeyCode.Q)) // если нажата q то вменить оружие
        {
            SwitchWeapon(); // смена оружия
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Power Up")) // если встречен тэг power up 
        {
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
        Debug.LogError("MOVE2 " + transform.position.z);
        playerMove.Tick(characterController, gravity); // выполнить движение по направлению
        Debug.LogError("MOVE3 " + transform.position.z);
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
        if (healthDisplay != null)
        {
            healthDisplay.text = "HP: " + health; // отобразить ХП на экране
        }
        else
        {
            healthDisplay = GameObject.FindGameObjectWithTag("Health").GetComponent<Text>();
        }
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
        ChangeHealth(5); // Добавить 5 ХП
        Destroy(other.gameObject); // Разрушить объект
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
        if (ribovTimer == null)
        {
            FishBooster[] obj = Resources.FindObjectsOfTypeAll<FishBooster>(); // Найти в ресурсах объекты с типом FishBooster
            foreach (var item in obj)
            {
                if (item.name.Equals("Power Up Timer")) // Если у объекта название совпало
                {
                    ribovTimer = item; // Присвоить таймеру элемент
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
        this.health = data.currentHpPlayer; // Загрзить здоровье из сохранения
        this.flipRight = data.isFlippedRight; // Загрузить поворт из сохранения

        characterController.enabled = false;
        vectorToSafe = data.currentPlacePlayer; // Загрузить текущие координаты из сохранения
        vector = data.currentPlacePlayer;
        transform.position = data.currentPlacePlayer; // Загрузить текущие координаты из сохранения
        characterController.enabled = true;
    }

    public void SaveData(ref DataGame data)
    {
        data.currentHpPlayer = (int) this.health; // Сохранить здоровье
        data.isFlippedRight = flipRight; // Сохранить поворт
        data.currentPlacePlayer = vectorToSafe; // Сохранить текущие координаты
    }
}



