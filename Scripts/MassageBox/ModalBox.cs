using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public abstract class ModalBox : MonoBehaviour
{


    public Text Title;
    public Text Message;
    public Button Button;

	bool isSetup;

    public RectTransform Panel;
    Transform buttonParent;





    // This code has to be run here so that layout has already happened and preferredHeights have been calculated.
    void FixedUpdate()
    {
        if (!isSetup)
        {
            isSetup = true;

            if (Title != null)
            {
				var text = Title.GetComponentInChildren<Text>();
                int textSize = Mathf.RoundToInt(Screen.width / 640f * 30f);
                text.fontSize = textSize;
                var layoutElement = Title.GetComponent<LayoutElement>();
                layoutElement.minHeight = Title.preferredHeight; // Set the min height to the preferred height so that the parent dialog box vertical layout can resize correctly.
				Title.alignment = TextAnchor.MiddleCenter;
                text.fontStyle = FontStyle.Normal;
            }

            if (Message != null)
            {
				var text = Message.GetComponentInChildren<Text>();
                int textSize = Mathf.RoundToInt(Screen.width / 640f * 30f);
                text.fontSize = textSize;
                var layoutElement = Message.GetComponent<LayoutElement>();
                layoutElement.minHeight = Message.preferredHeight; // Set the min height to the preferred height so that the parent dialog box vertical layout can resize correctly.
				Message.alignment = TextAnchor.MiddleCenter;
            }

            if (buttonParent != null)
            {
                // Set the min height of each button
                foreach (Transform button in buttonParent)
                {
                    var text = button.GetComponentInChildren<Text>();
                    int textSize = Mathf.RoundToInt(Screen.width / 640f * 30f);
                    text.fontSize = textSize;
                    if (text != null)
                    {
                        var buttonLayoutElement = button.GetComponent<LayoutElement>();
                        if (buttonLayoutElement != null)
                            buttonLayoutElement.minHeight = text.preferredHeight + 10;

                        var buttonParentLayoutElement = buttonParent.GetComponent<LayoutElement>();
                        if (buttonParentLayoutElement != null)
                            buttonParentLayoutElement.minHeight = Mathf.RoundToInt(Screen.width / 640f * 70f);
                    }
                }
            }

            if (Panel != null)
            {
                // Center the panel to it's new height
                var group = Panel.GetComponent<VerticalLayoutGroup>();
                float xSize = Screen.width * 0.938f;
                float ySize = Screen.height * 0.22f;
                Panel.sizeDelta = new Vector2(xSize, ySize);
            }
        }
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public virtual void Close()
    {
        Destroy(gameObject);
    }

    protected void SetText(string message, string title)
    {
        if (Button != null)
            buttonParent = Button.transform.parent;

        if (Title != null)
        {
            if (!String.IsNullOrEmpty(title))
            {
                Title.text = MessageBox.LocalizeTitleAndMessage ? MessageBox.Localize(title) : title;
            }
            else
            {
                Destroy(Title.gameObject);
                Title = null;
            }
        }

        if (Message != null)
        {
            if (!String.IsNullOrEmpty(message))
            {
                Message.text = MessageBox.LocalizeTitleAndMessage ? MessageBox.Localize(message) : message;
            }
            else
            {
                Destroy(Message.gameObject);
                Message = null;
            }
        }
    }
}
