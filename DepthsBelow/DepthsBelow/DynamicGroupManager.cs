﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DepthsBelow
{
	public class DynamicGroupManager
	{
		public class Group
		{
			public List<Entity> Entities;
			public float Panic;
			public float MaxPanic
			{
				get
				{
					float maxPanic = 0;
					foreach (var entity in Entities)
					{
						var stats = entity.GetComponent<Component.Stat>();
						if (stats != null && stats.Life > 0)
							maxPanic += stats.MaxPanic;
					}
					return maxPanic;
				}
			}
		}

		private List<Entity> entities;

		public float MaxRange;

		public List<Group> Groups; 

		public DynamicGroupManager(List<Entity> entities, float maxRange)
		{
			this.entities = entities;
			this.MaxRange = maxRange;

			UpdateGroups();
		}

		public void UpdateGroups()
		{
			if (Groups == null)
				Groups = new List<Group>();

			var newGroups = CreateGroups(MaxRange);

			foreach (var newGroup in newGroups)
			{
				for (int index = 0; index < newGroup.Entities.Count; index++)
				{
					var entity = newGroup.Entities[index];

					Group oldGroup = null;
					// Find the group the entity belonged to before
					foreach (var group in Groups)
					{
						if (@group.Entities.Contains(entity))
						{
							oldGroup = @group;
							break;
						}
					}

					if (oldGroup != null)
					{
						if (entity.GetComponent<Component.Stat>().Life > 0)
						{
							newGroup.Panic += (oldGroup.Panic/oldGroup.MaxPanic)*(float) entity.GetComponent<Component.Stat>().MaxPanic;
							//if (entity.GetComponent<Component.Stat>().Life > 0)
							oldGroup.Panic -= (oldGroup.Panic/oldGroup.MaxPanic)*(float) entity.GetComponent<Component.Stat>().MaxPanic;
							oldGroup.Entities.Remove(entity);
						}
					}
					else
						newGroup.Panic = 125;
				}
			}

			Groups = newGroups;
		}

		private List<Group> CreateGroups(float maxRange)
		{
			var alreadyGrouped = new List<Entity>();
			var groups = new List<Group>();

			for (int index = 0; index < entities.Count; index++)
			{
				var soldier = entities[index];

				if (!alreadyGrouped.Contains(soldier) && soldier.GetComponent<Component.Stat>().Life > 0)
				{
					var group = new List<Entity>();
					GetNearChain(ref group, soldier, maxRange);
					alreadyGrouped.AddRange(group);

					if (group.Count > 0)
						groups.Add(new Group() { Entities = group });
				}
			}

			return groups;
		}

		// Get all entities who are close to each other in a chain
		private void GetNearChain(ref List<Entity> group, Entity entity, float maxRange)
		{
			for (int index = 0; index < entities.Count; index++)
			{
				var nearSoldier = entities[index];

				if (!group.Contains(nearSoldier) && nearSoldier.GetComponent<Component.Stat>().Life > 0)
				{
					float distance = Vector2.Distance(nearSoldier.Transform.World + nearSoldier.Transform.World.Origin,
														  entity.Transform.World + entity.Transform.World.Origin);
					if (distance <= maxRange)
					{
						group.Add(nearSoldier);
						GetNearChain(ref group, nearSoldier, maxRange);
					}
				}
			}
		}
	}
}
