using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

class ExtensionComponent : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ExtensionComponent, UxmlTraits> { }

    public ExtensionComponent()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
    }
    
    private void OnAttachToPanel(AttachToPanelEvent args)
    {
        this.RegisterCallback<ClickEvent>(OnClicked);
    }
    
    

    private void OnClicked(ClickEvent e)
    {
        Debug.Log("click");
    }
    
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_String =
            new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };
        UxmlIntAttributeDescription m_Int =
            new UxmlIntAttributeDescription { name = "int-attr", defaultValue = 2 };

        
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ate = ve as ExtensionComponent;

            ate.stringAttr = m_String.GetValueFromBag(bag, cc);
            ate.intAttr = m_Int.GetValueFromBag(bag, cc);
            
            ate.CaptureMouse();
        }
    }

    public string stringAttr { get; set; }
    public int intAttr { get; set; }
}