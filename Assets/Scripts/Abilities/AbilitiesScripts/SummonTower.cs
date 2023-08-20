using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Ability/SummonTower")]
public class SummonTower : Ability
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Vector2 summonOffset;
    [SerializeField] private float cooldown;

    [Header("TOWER DATA")]
    [SerializeField] private int hp;
    [SerializeField] private LayerMask collisionMask;

    private bool facingRight;
    private PlayerInputs inputs;
    private Tower currentTower;
    private bool canActivate = true;
    private float lastActivationTime;

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
        if (!canActivate || currentTower == null) return;

        PlayerController player = parent.GetComponent<PlayerController>();

        if (!player.grounded) return;
        
        Vector2 summonPos;

        if (facingRight)
            summonPos = new Vector2(player.transform.position.x + summonOffset.x, player.transform.position.y + summonOffset.y);
        else
            summonPos = new Vector2(player.transform.position.x - summonOffset.x, player.transform.position.y + summonOffset.y);

        parent.GetComponent<Animator>().SetTrigger("Defending");
        currentTower.Activate(summonPos);

        canActivate = false;
        lastActivationTime = Time.time;
    }

    public override void UpdateAbility()
    {
        base.UpdateAbility();

        if (!canActivate && Time.time >= lastActivationTime + cooldown)
            canActivate = true;
    }

    public override void Pick(Character picker)
    {
        base.Pick(picker);

        currentTower = Instantiate(towerPrefab).GetComponent<Tower>();
        currentTower.Initialize(hp, collisionMask);

        canActivate = true;
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
