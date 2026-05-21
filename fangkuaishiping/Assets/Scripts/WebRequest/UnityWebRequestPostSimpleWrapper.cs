
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class UnityWebRequestPostSimpleWrapper 
{
	private MonoBehaviour mMono;
	public UnityWebRequestPostSimpleWrapper(MonoBehaviour mono)
	{
		mMono = mono;
	}

	#region StartUnityWebRequestPost
	public Coroutine StartUnityWebRequestPost(string url, string postData, string requestHeaderStr, Action<bool, string> onPostCallback)
	{
		if (mMono == null)
		{
			//Debug.LogError(GetType() + "/StartUnityWebRequestPost()/ mMono can not be null");
			return null;
		}

		return mMono.StartCoroutine(UnityWebRequestPost(url, postData, requestHeaderStr, onPostCallback));
	}
	#endregion

	#region StartUnityWebRequestFormPost
	public Coroutine StartUnityWebRequestFormPost(string url, WWWForm form, string requestHeaderStr, Action<bool, string> onPostCallback)
	{
		if (mMono == null)
		{
			//Debug.LogError(GetType() + "/StartUnityWebRequestPost()/ mMono can not be null");
			return null;
		}

		return mMono.StartCoroutine(UnityWebRequestFormPost(url, form, requestHeaderStr, onPostCallback));
	}
    #endregion

    #region UnityWebRequestPost
    IEnumerator UnityWebRequestPost(string url, string postData, string requestHeaderStr, Action<bool, string> onPostCallback)
	{
		

		using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
		{
			
			//Debug.Log(GetType() + "/UnityWebRequestPost()/ url : " + webRequest.url);
			//Debug.Log(GetType() + "/UnityWebRequestPost()/ postData : " + postData);
			

			byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
			webRequest.uploadHandler = new UploadHandlerRaw(postBytes);
			webRequest.downloadHandler = new DownloadHandlerBuffer();
			string newhead= "Bearer " + requestHeaderStr;
			webRequest.SetRequestHeader("authorization", newhead);
			string head_ = "en ";
			webRequest.SetRequestHeader("think-lang", head_);
			yield return webRequest.SendWebRequest();
			string response = null;
			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				response = webRequest.error;
				//Debug.LogError(GetType() + "/UnityWebRequestPost()/ error : " + webRequest.error);

				if (onPostCallback != null)
				{
					onPostCallback.Invoke(false, response);

				}
			}
			else
			{

				response = webRequest.downloadHandler.text;

				//Debug.Log(GetType() + "/UnityWebRequestPost()/ success : " + response);

				if (onPostCallback != null)
				{
					onPostCallback.Invoke(true, response);

				}
			}
		}

	}
	#endregion

	#region UnityWebRequestFormPost
	IEnumerator UnityWebRequestFormPost(string url, WWWForm form, string requestHeaderStr, Action<bool, string> onPostCallback)
	{
		using (UnityWebRequest webRequest =UnityWebRequest.Post(url,form))
		{

			//Debug.Log(GetType() + "/UnityWebRequestPost()/ url : " + webRequest.url);
			//Debug.Log(GetType() + "/UnityWebRequestPost()/ postData : " + postData);
			webRequest.downloadHandler = new DownloadHandlerBuffer();
			string newhead = "Bearer " + requestHeaderStr;
			webRequest.SetRequestHeader("authorization", newhead);
			string head_ = "en ";
			webRequest.SetRequestHeader("think-lang", head_);




			yield return webRequest.SendWebRequest();
			string response = null;
			if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				response = webRequest.error;
				//Debug.LogError(GetType() + "/UnityWebRequestPost()/ error : " + webRequest.error);

				if (onPostCallback != null)
				{
					onPostCallback.Invoke(false, response);

				}
			}
			else
			{

				response = webRequest.downloadHandler.text;

				//Debug.Log(GetType() + "/UnityWebRequestPost()/ success : " + response);

				if (onPostCallback != null)
				{
					onPostCallback.Invoke(true, response);

				}
			}
		}

	}
	#endregion


}
