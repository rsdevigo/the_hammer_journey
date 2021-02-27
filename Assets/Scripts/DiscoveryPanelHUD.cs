using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DiscoveryPanelHUD : MonoBehaviour
{
  [SerializeField] public NetworkDiscoveryExt networkDiscovery;
  [SerializeField] public ScrollRect scrollView;
  [SerializeField] public GameObject ServerItemPrefab;
  readonly Dictionary<long, DiscoveryResponse> _servers = new Dictionary<long, DiscoveryResponse>();

  public void OnListServerButtonPressed()
  {
    _servers.Clear();
    networkDiscovery.StartDiscovery();
  }

  public void OnCreateServerButtonPressed()
  {
    _servers.Clear();
    ClearList();
    NetworkManager.singleton.StartHost();
    networkDiscovery.AdvertiseServer();
  }

  public void OnLogoutButtonPressed()
  {
    LoginController.Logout();
  }

  public void OnServerDiscover(DiscoveryResponse response)
  {
    _servers[response.serverId] = response;
    Draw();
  }

  public void Connect(DiscoveryResponse info)
  {
    NetworkManager.singleton.StartClient(info.uri);
  }

  public void Draw()
  {
    if (scrollView.content != null)
    {
      ClearList();
      foreach (DiscoveryResponse server in _servers.Values)
      {
        GameObject serverItem = Instantiate(ServerItemPrefab, scrollView.content.transform);
        serverItem.GetComponentInChildren<Text>().text = server.EndPoint.Address.ToString();
        serverItem.GetComponentInChildren<Button>().onClick.AddListener(() => { Connect(server); });
      }
    }

  }

  public void ClearList()
  {
    if (scrollView.content != null)
    {
      foreach (Transform child in scrollView.content.transform)
        Destroy(child.gameObject);
    }
  }

}
