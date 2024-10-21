using UnityEngine;
using MaloProduction.CustomAttributes;

public class AttributesTuto : MonoBehaviour
{
//Just to remove the warning in the console: 'This variable is not used.
#pragma warning disable CS0414

    [Header("Show If")]
    [SerializeField] private bool haveLife;
    [ShowIf("haveLife"), SerializeField, Range(0f, 100f)] private float life;

    [Header("ShowIfFunction")]
    [SerializeField] private float damage;
    [ShowIfFunction("ShowDamageMulitplierInspector"), SerializeField, Range(0f, 10f)] private float damageMultiplier;
    private bool ShowDamageMulitplierInspector()
    {
        return damage >= 5f;
    }

    [Header("ShowIfMultiples")]
    [SerializeField] private bool haveResistance;
    [SerializeField] private bool haveArmor;
    [ShowIfMultiples("haveResistance", "haveArmor"), SerializeField, Range(0f, 100f)] private float resistanceMultiplier;

    [Header("Color")]
    [Color(255f, 255f, 0f, 200f, true, ColorUsage.Background), SerializeField] private float mana;

    [Header("Sprite")]
    [Sprite, SerializeField] private Sprite characterIco;

    [Header("Size")]
    [Size(125), SerializeField] private string characterName;

    [Header("Title")]
    [Title("Player Anim")]
    [SerializeField] private Animator animator;

    [Header("Margin")]
    [Margin(20f, 20f, 20f, 20f), SerializeField] private float speed;

    [Header("Spacing")]
    [Spacing(5, 0.5f, 0.25f, 0f)]
    [SerializeField] private bool isDead;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private string ip = "0000.0000.0000.0000";

    [Header("Button")]
    [SerializeField, ReadOnly, TextArea] private string temp = "Just to ensure that the header is displayed in the inspector, this variable is in no way necessary for the correct functioning of the attribute.";
    [Button("HelloWorld")]
    private void HelloWorld()
    {
        Debug.Log("Hello World");
    }
#pragma warning restore CS0414
}