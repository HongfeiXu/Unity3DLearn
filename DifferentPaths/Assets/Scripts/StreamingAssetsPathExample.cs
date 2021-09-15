using UnityEngine;
using System.IO;
using UnityEngine.Video;
using System.Collections;

// Application-streamingAssetsPath example.
//
// Play a video and let the user stop/start it.
// The video location is StreamingAssets. The video is
// played on the camera background.

public class StreamingAssetsPathExample : MonoBehaviour
{
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private string status;

    void Start()
    {
        GameObject cam = GameObject.Find("Main Camera");
        videoPlayer = cam.AddComponent<UnityEngine.Video.VideoPlayer>();

        Debug.Log("streamingAssetsPath = " + Application.streamingAssetsPath);
		// Obtain the location of the video clip.
		videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "test1.mp4");

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Do not show the video until the user needs it.
        videoPlayer.Pause();

        status = "Press to play";

        TestStreamingAssetsPath();
	}


    void TestStreamingAssetsPath()
	{
        // 安卓平台不能用System.IO.File来直接读取StreamingAssetsPath下的文件内容，因为这些内容被打成jar包了
        // Application.streamingAssetsPath： jar:file:///data/app/com.DefaultCompany.DifferentPaths-2/base.apk!/assets
        // 所以下面的代码会报错

        //var bytes =  File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, "test1.mp4"));

        // 需要使用UnityWebRequest来读取
        StartCoroutine(ReadFromStreamingAssets());
    }

    IEnumerator ReadFromStreamingAssets()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "test1.mp4");
        byte[] result;
        if (filePath.Contains("://") || filePath.Contains(":///"))  // 安卓会走这里
        {
            Debug.Log("ReadFromStreamingAssets goes to UnityWebRequest");
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.data;
        }
        else
		{
            Debug.Log("ReadFromStreamingAssets goes to File.ReadAllBytes");
            result = File.ReadAllBytes(filePath);
        }
        Debug.Log("Application.persistentDataPath = " + Application.persistentDataPath);
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "test1.mp4"), result);
	}

    void OnGUI()
    {
        GUIStyle buttonWidth = new GUIStyle(GUI.skin.GetStyle("button"));
        buttonWidth.fontSize = 18 * (Screen.width / 800);

        if (GUI.Button(new Rect(100, 100, 200, 80), status, buttonWidth))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                status = "Press to play";
            }
            else
            {
                videoPlayer.Play();
                status = "Press to pause";
            }
        }
    }
}