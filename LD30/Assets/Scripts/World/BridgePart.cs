using UnityEngine;
using System.Collections;
using System;

public class BridgePart : MonoBehaviour {

	public enum BridgePartType
	{
		Straight, 
		Corner,
		TJunction, 
		Cross,
	}

	[Flags]
	enum Direction : int
	{
		None = 0,
		Up = 1,
		Down = 2, 
		Left = 4, 
		Right = 8,
	}

	[SerializeField]
	Sprite[] m_sprites;
	
	SpriteRenderer m_renderer;

	public BridgePart Up {get;set;}
	public BridgePart Down {get;set;}
	public BridgePart Left {get;set;}
	public BridgePart Right {get;set;}

	void Awake()
	{
		m_renderer = GetComponent<SpriteRenderer>();
	}

	public void UpdateType()
	{
		//Nasty
		Direction dir = 0;
		if(Up != null) dir |= Direction.Up;
		if(Down != null) dir |= Direction.Down;
		if(Left != null) dir |= Direction.Left;
		if(Right != null) dir |= Direction.Right;

		if(dir == (Direction.Left) || dir == (Direction.Right) || dir == (Direction.Left | Direction.Right))
		{
			PartType = BridgePartType.Straight;
			transform.localEulerAngles = new Vector3(0, 0, 0.0f);
		}
		else if(dir == Direction.Down || dir == Direction.Up || dir == (Direction.Up | Direction.Down))
		{
			PartType = BridgePartType.Straight;
			transform.localEulerAngles = new Vector3(0, 0, 90.0f);
		}

		//Corner
		else if(dir == (Direction.Up | Direction.Left))
		{
			PartType = BridgePartType.Corner;
			transform.localEulerAngles = new Vector3(0, 0, 0.0f);

		}
		else if(dir == (Direction.Left | Direction.Down))
		{
			PartType = BridgePartType.Corner;
			transform.localEulerAngles = new Vector3(0, 0, 90.0f);
		}
		else if(dir == (Direction.Down | Direction.Right))
		{
			PartType = BridgePartType.Corner;
			transform.localEulerAngles = new Vector3(0, 0, 180.0f);
		}
		else if(dir == (Direction.Right | Direction.Up))
		{
			PartType = BridgePartType.Corner;
			transform.localEulerAngles = new Vector3(0, 0, -90.0f);
		}

		//T Junction
		else if(dir == (Direction.Up | Direction.Left | Direction.Down))
		{
			PartType = BridgePartType.TJunction;
			transform.localEulerAngles = new Vector3(0, 0, -90.0f);
		}
		else if(dir == (Direction.Left | Direction.Down | Direction.Right))
		{
			PartType = BridgePartType.TJunction;
			transform.localEulerAngles = new Vector3(0, 0, 180.0f);
		}
		else if(dir == (Direction.Down | Direction.Right | Direction.Up))
		{
			PartType = BridgePartType.TJunction;
			transform.localEulerAngles = new Vector3(0, 0, 90.0f);
		}
		else if(dir == (Direction.Right | Direction.Up | Direction.Left))
		{
			PartType = BridgePartType.TJunction;
			transform.localEulerAngles = new Vector3(0, 0, 0.0f);
		}

		//Cross
		else PartType = BridgePartType.Cross;
	}

	public BridgePartType PartType 
	{
		set { m_renderer.sprite = m_sprites[(int)value]; } 
	}	
}
