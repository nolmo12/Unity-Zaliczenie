using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Player : Mob, IGiveItem
{
    public Camera mainCamera;
    public GameObject playerCharacter;

    InputAction moveAction;
    InputAction crouchAction;
    InputAction sprintAction;
    InputAction fireAction;
    InputAction getNextItem;
    InputAction getPrevItem;
    InputAction Reload;

    public LinkedList<Item> Items = new LinkedList<Item>();
    public LinkedListNode<Item> CurItem;
    void Start()
    {
        Init();
        moveAction = InputSystem.actions.FindAction("Move");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        getNextItem = InputSystem.actions.FindAction("Next");
        getPrevItem = InputSystem.actions.FindAction("Previous");
        Reload = InputSystem.actions.FindAction("Reload");

        fireAction = InputSystem.actions.FindAction("Attack");

        GameDirector.Instance.PlayerPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead())
        {
            Death();
        }
        else
        {
            GameDirector.Instance.PlayerPos = transform.position;
            HandlePlayerMovement();
            HandlePlayerRotation();
            HandleItems();
        }
    }

    public override void Death()
    {
        StopAllCoroutines();
        Destroy(playerCharacter);
        GameObject[] TopPanelObjects = GameObject.FindGameObjectsWithTag("TopPanel");
        
        GameDirector.Instance.EndScreen.SetActive(true);

        foreach (GameObject obj in TopPanelObjects)
        {
            Debug.Log(obj.name);
            obj.SetActive(false);
        }
    }

    void HandlePlayerMovement()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        Vector3 moveVector = new Vector3(moveValue.x, 0, moveValue.y) * Speed * Time.deltaTime;

        Vector3 targetPosition = Rigidbody.position + moveVector;

        Rigidbody.position = Vector3.MoveTowards(Rigidbody.position, targetPosition, Speed * Time.deltaTime);
    }

    void HandlePlayerRotation()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = (hit.point - playerCharacter.transform.position).normalized;

            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }

    void HandleItems()
    {
        if (getNextItem.WasPressedThisFrame())
        {
            if (CurItem.Next != null)
            {
                CurItem.Value.gameObject.SetActive(false);
                CurItem.Value.StopAllCoroutines();
                CurItem = CurItem.Next;
                CurItem.Value.gameObject.SetActive(true);
                Debug.Log("Selected next item: " + CurItem.Value.Name);
            }
            else
            {
                Debug.Log("No more items in the list.");
            }
        }

        if (getPrevItem.WasPressedThisFrame())
        {
            if (CurItem.Previous != null)
            {
                CurItem.Value.gameObject.SetActive(false);
                CurItem.Value.StopAllCoroutines();
                CurItem = CurItem.Previous;
                CurItem.Value.gameObject.SetActive(true);
                Debug.Log("Selected previous item: " + CurItem.Value.Name);
            }
            else
            {
                Debug.Log("No previous items in the list.");
            }
        }

        if (CurItem.Value is RangedWeapon weapon)
        {
            if (fireAction.IsPressed() && !CurItem.Value.CheckIfInUse())
            {
                StartCoroutine(CurItem.Value.Use(this));
            }

            if(Reload.WasPressedThisFrame() && weapon.Ammo != weapon.MaxAmmo)
            {
                StartCoroutine(weapon.Reload());
            }
        }
        else
        {
            if (fireAction.WasPressedThisFrame() && !CurItem.Value.CheckIfInUse())
            {
                StartCoroutine(CurItem.Value.Use(this));
            }
        }
    }

    public override void Init()
    {
        base.Init();

        GameObject loadedPrefab = GiveItem("Weapons/Prefabs/Pistol", new Vector3(5.4f, 18f, 2.15f));
        CurItem = Items.Last;
        foreach (Item item in Items)
        {
            item.gameObject.SetActive(false);
        }

        CurItem.Value.gameObject.SetActive(true);
    }

    public GameObject GiveItem(string path, Vector3 pos)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>(path);

        if (loadedPrefab == null)
        {
            return null;
        }

        GameObject itemGameObject = Instantiate(loadedPrefab, new Vector3(0, 0, 0), transform.rotation);

        itemGameObject.transform.parent = playerCharacter.transform;

        itemGameObject.transform.localPosition = pos;

        itemGameObject.tag = "Player";
        itemGameObject.SetActive(false);

        Items.AddLast(itemGameObject.GetComponent<Item>());

        return itemGameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        RangedWeapon weapon = collision.gameObject.GetComponent<RangedWeapon>();
        if (weapon != null)
        {
            foreach (Item item in Items)
            {
                if(item.Name == weapon.Name || weapon.Name + " (Reloading)" == item.Name)
                {
                    return;
                }
            }
            GiveItem($"Weapons/Prefabs/{weapon.Name}", new Vector3(5.4f, 18f, 2.15f));
            Destroy(collision.gameObject);
        }
    }

}
