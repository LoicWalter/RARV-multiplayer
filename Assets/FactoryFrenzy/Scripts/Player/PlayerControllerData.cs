using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerControllerData : IEquatable<PlayerControllerData>, INetworkSerializable
{
  public ulong clientId;

  public bool Equals(PlayerControllerData other)
  {
    return
        clientId == other.clientId;
  }

  public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
  {
    serializer.SerializeValue(ref clientId);
  }

}