using System.Collections.Generic;
using System.Numerics;

namespace RoleplayingQuestCore
{
    /// <summary>
    /// A single recorded point along the path the NPC will walk during a tail objective.
    /// </summary>
    public class PathWaypoint
    {
        float _timestamp;       // Seconds since recording started
        Vector3 _position;
        Vector3 _rotation;
        ushort _emoteId;        // 0 = no emote at this point

        public float Timestamp { get => _timestamp; set => _timestamp = value; }
        public Vector3 Position { get => _position; set => _position = value; }
        public Vector3 Rotation { get => _rotation; set => _rotation = value; }
        public ushort EmoteId { get => _emoteId; set => _emoteId = value; }
    }

    /// <summary>
    /// All data needed to drive a "tail the NPC" objective.
    /// The quest creator records a walking path; the NPC replays it at runtime
    /// while periodically turning around to check for the player.
    /// </summary>
    public class TailObjectiveData
    {
        string _npcName = "";
        List<PathWaypoint> _waypoints = new List<PathWaypoint>();
        Vector3 _playerStartPosition;
        Vector3 _playerStartRotation;
        float _detectionRadius = 15f;
        float _detectionConeAngle = 120f;   // Full cone in degrees
        float _lookBackMinInterval = 8f;    // Seconds between look-backs (min)
        float _lookBackMaxInterval = 20f;   // Seconds between look-backs (max)
        float _lookBackDuration = 3f;       // How long the NPC stares behind them
        float _npcSpeed = 5f;
        float _minimumTailDistance = 3f;    // Fail if closer than this
        float _maximumTailDistance = 45f;   // Fail if further than this

        public string NpcName { get => _npcName; set => _npcName = value; }
        public List<PathWaypoint> Waypoints { get => _waypoints; set => _waypoints = value; }
        public Vector3 PlayerStartPosition { get => _playerStartPosition; set => _playerStartPosition = value; }
        public Vector3 PlayerStartRotation { get => _playerStartRotation; set => _playerStartRotation = value; }
        public float DetectionRadius { get => _detectionRadius; set => _detectionRadius = value; }
        public float DetectionConeAngle { get => _detectionConeAngle; set => _detectionConeAngle = value; }
        public float LookBackMinInterval { get => _lookBackMinInterval; set => _lookBackMinInterval = value; }
        public float LookBackMaxInterval { get => _lookBackMaxInterval; set => _lookBackMaxInterval = value; }
        public float LookBackDuration { get => _lookBackDuration; set => _lookBackDuration = value; }
        public float NpcSpeed { get => _npcSpeed; set => _npcSpeed = value; }
        public float MinimumTailDistance { get => _minimumTailDistance; set => _minimumTailDistance = value; }
        public float MaximumTailDistance { get => _maximumTailDistance; set => _maximumTailDistance = value; }

        // Runtime-only flag (not serialized to disk)
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool PathCompleted { get; set; }
    }
}
