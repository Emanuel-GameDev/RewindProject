using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Ability/SummonTower")]
public class SummonTower : Ability
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Vector2 summonOffset;
    [Tooltip("Radius of the sphere generated to check if summon pos is inside other objects")]
    [SerializeField] private float summonSphereRadius;
    [SerializeField] private LayerMask summonMask;
    [SerializeField] private AudioClip summonClip;
    [SerializeField] private AudioClip dismissClip;

    [Header("TOWER DATA")]
    [SerializeField] private int hp;
    [SerializeField] private LayerMask collisionMask;

    private bool facingRight;
    private PlayerInputs inputs;
    private Tower currentTower;
    private bool canActivate = true;
    private float lastActivationTime;
    private bool active = false;
    private Character character;

    private void OnEnable()
    {
        inputs = new PlayerInputs();

        if (!inputs.Player.enabled)
            inputs.Player.Enable();

        inputs.Player.Walk.performed += CheckHorizontalFacing;
        inputs.Player.Run.performed += CheckHorizontalFacing;

    }

    private void OnDisable()
    {
        inputs.Player.Walk.performed -= CheckHorizontalFacing;
        inputs.Player.Run.performed -= CheckHorizontalFacing;
    }

    public override void Activate1(GameObject parent)
    {
        if (!canActivate || currentTower == null || active) return;

        character = parent.GetComponent<Character>();
        PlayerController player = parent.GetComponent<PlayerController>();

        Vector2 summonPos;
        float summonRot;

        bool gravityDown = player.IsGravityDownward();

        //GameObject contact = currentTower.GetContactPoint(player.transform.position, gravityDown);
        Transform playerGround = player.transform.GetChild(0);

        //if (contact == null)
        //{
        //    Debug.LogError("NO Contact found, can't activate ability");
        //    return;
        //}

        if (gravityDown)
        {
            if (facingRight)
                summonPos = new Vector2(playerGround.transform.position.x + summonOffset.x, playerGround.transform.position.y + summonOffset.y);
            else
                summonPos = new Vector2(playerGround.transform.position.x - summonOffset.x, playerGround.transform.position.y + summonOffset.y);

            summonRot = 0f;
        }
        else
        {
            if (facingRight)
                summonPos = new Vector2(playerGround.transform.position.x + summonOffset.x, playerGround.transform.position.y - summonOffset.y);
            else
                summonPos = new Vector2(playerGround.transform.position.x - summonOffset.x, playerGround.transform.position.y - summonOffset.y);

            summonRot = 180f;
        }

        if (!player.grounded || !currentTower.CanBeActivated(summonPos, summonSphereRadius, summonMask)) return;

        parent.GetComponent<Animator>().SetTrigger("Defending");

        currentTower.Activate(this, summonPos, summonRot, facingRight);
        parent.GetComponent<MainCharacter_SoundsGenerator>().PlaySound(summonClip);

        active = true;
    }

    public void DismissAudio()
    {
        character.gameObject.GetComponent<MainCharacter_SoundsGenerator>().PlaySound(dismissClip);
    }

    public void StartCooldown()
    {
        canActivate = false;
        lastActivationTime = Time.time;
        active = false;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (!canActivate && Time.time >= lastActivationTime + cooldownTime)
        {
            canActivate = true;
            lastActivationTime = 0f;
        }
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        currentTower = Instantiate(towerPrefab).GetComponent<Tower>();
        currentTower.Initialize(hp, collisionMask);

        canActivate = true;
        active = false;
    }

    private void CheckHorizontalFacing(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();

        if (value < 0)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

}
