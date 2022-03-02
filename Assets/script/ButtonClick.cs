using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [HideInInspector] public CheckLetter checkLetter;
    [SerializeField] Text text;

    [SerializeField] ParticleSystem particle;

    [SerializeField] private Image image;

    private void Start()
    {
        EventManager.SecretWordGuessed.AddListener(FadeOff);
    }

    void FadeOff()
    {
        image.enabled = false;
        text.enabled = false;
    }

    public void Click() // игрок нажал на кнопку-букву
    {
        image.enabled = false;
        //  particle.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0.75f);
        var main = particle.main;
        if (checkLetter.CheckThisLetter(text.text))
            main.startColor = Color.green;
        else
            main.startColor = Color.red;

        text.enabled = false;
        particle.Play();

        //Destroy(gameObject, 2);
    }
}