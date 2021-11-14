using UnityEngine;

public class LinkOpener : MonoBehaviour
{
   public void OpenFacebook()
    {
        Application.OpenURL("https://facebook.com/LlamAcademyOfficial");
    }

    public void OpenYoutube()
    {
        Application.OpenURL("https://youtube.com/c/LlamAcademy");
    }
}
