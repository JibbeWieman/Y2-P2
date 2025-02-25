using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.FPS.Game;

public class UIManager : Manager
{
    private GraphManager graphManager;
    private Dictionary<Node, Button> skillNodeButtonMap = new();

    [SerializeField]
    private TextMeshProUGUI skillPointCount;

    public override void Start()
    {
        base.Start();

        //EventManager.AddListener<SkillPointsChangedEvent>(UpdateSkillPointCount);
        //EventManager.AddListener<SkillTreeUIClosedEvent>(HideAllTooltips);
        graphManager = FindAnyObjectByType<GraphManager>();

        BindSkillButtons();
    }

    /// <summary>
    /// Binds each skill node in the GraphManager to a button and sets up event listeners for pointer events.
    /// </summary>
    private void BindSkillButtons()
    {
        foreach (var node in graphManager.Nodes)        // Chose not to do this since I would've had to add .Value behind every skillNode variable
        //for (int i = 0; i < graphManager.Nodes.Count; i++)
        {
            Node skillNode = node.Value;                //graphManager.Nodes[i];
            Button skillButton = skillNode.skillButton;
            skillNodeButtonMap[skillNode] = skillButton;    // Map the Node to its Button for easy access

            // Create commands
            var showTooltipCommand = new ShowTooltipCommand(skillNode.skillTooltip);
            var hideTooltipCommand = new HideTooltipCommand(skillNode.skillTooltip);

            // Add pointer enter and exit event listeners
            EventTrigger trigger = skillButton.gameObject.AddComponent<EventTrigger>();
            AddPointerEvent(trigger, EventTriggerType.PointerEnter, showTooltipCommand);
            AddPointerEvent(trigger, EventTriggerType.PointerExit, hideTooltipCommand);
        }
    }
    private void AddPointerEvent(EventTrigger trigger, EventTriggerType eventType, Command command)
    {
        EventTrigger.Entry entry = new() { eventID = eventType };
        entry.callback.AddListener((data) => command.Execute());
        trigger.triggers.Add(entry);
    }

    /*public void HideAllTooltips(SkillTreeUIClosedEvent evt)
    {
        foreach (Node node in graphManager.Nodes.Values)
        {
            node.skillTooltip.HideTooltip();
        }
    }

    private void UpdateSkillPointCount(SkillPointsChangedEvent evt) => 
        skillPointCount.text = $"Skill Points: {evt.SkillPoints}";
    */

    private void OnDestroy()
    {
        //EventManager.RemoveListener<SkillPointsChangedEvent>(UpdateSkillPointCount);
        //EventManager.RemoveListener<SkillTreeUIClosedEvent>(HideAllTooltips);
    }

    public override void Pause()
    {
        throw new System.NotImplementedException();
    }
}
