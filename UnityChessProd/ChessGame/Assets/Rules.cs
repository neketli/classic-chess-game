using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessLib;

public class Rules : MonoBehaviour
{
	Dictionary<string, GameObject> promotions;
	DragAndDrop dnd;
	Chess chess;

	string onPromotionMove;

	public Rules()
	{
		dnd = new DragAndDrop();
		chess = new Chess();
	}

	// Start is called before the first frame update
	void Start()
	{
		ShowFigures();
	}

	// Update is called once per frame
	void Update()
	{
		if (dnd.Action())
        {
			string from = GetSquare(dnd.PickedPosition);
			string to = GetSquare(dnd.DropedPosition);
			string figure = chess.GetFigure(from).ToString();
			string move = figure + from + to;
			if (figure == "P" && to[1] == '8')
            {
				move+= "Q";
				//promotion
            } else if (figure == "p" && to[1] == '1') move += "q";
			Debug.Log(move);
			chess = chess.Move(move);
			ShowFigures();
        }
	}

	string GetSquare(Vector2 position)
    {
		int x = System.Convert.ToInt32((position.x / 50.0)-5);
		int y = System.Convert.ToInt32((position.y / 50.0)-1);
		return ((char)('a' + x)).ToString() + (y + 1).ToString();
	}

	void ShowFigures()
	{
		int nr = 0;
		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				string figure = chess.GetFigure(x, y).ToString();
				if (figure == ".") continue;
				PlaceFigure($"box ({nr})", figure, x, y);
				nr++;
			}
		}
		for (; nr < 32; nr++)
			PlaceFigure($"box ({nr})", "q", -1, -1);
	}


	void PlaceFigure(string box, string figure, int x, int y)
	{
		//Debug.Log(box + " " + figure + " " + x + y);
		GameObject goBox = GameObject.Find(box);
		GameObject goFigure = GameObject.Find(figure);
		GameObject goSquare = GameObject.Find($"{y}{x}");

		var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
		var spriteBox = goBox.GetComponent<SpriteRenderer>();
		spriteBox.sprite = spriteFigure.sprite;

		goBox.transform.position = goSquare.transform.position;
	}
}

class DragAndDrop
{
	enum State
	{
		none,
		drag
	}

	public Vector2 PickedPosition { get; private set; }
	public Vector2 DropedPosition { get; private set; }


	State state;
	GameObject item;
	Vector2 offset;

	public DragAndDrop()
	{
		state = State.none;
		item = null;
	}

	public bool Action()
	{
		switch (state)
		{
			case State.none:
				if (IsMousePressed()) PickUp();
				break;
			case State.drag:
				if (IsMousePressed()) Drag();
				else
				{
					Drop();
					return true;
				}
				break;
		}
		return false;
	}

	Vector2 GetClickPosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	Transform GetItemAt(Vector2 position)
	{
		RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
		if (figures.Length == 0) return null;
		return figures[0].transform;
	}

	bool IsMousePressed()
	{
		return Input.GetMouseButton(0);
	}

	void PickUp()
	{
		Vector2 clickPosition = GetClickPosition();
		Transform clickedItem = GetItemAt(clickPosition);
		if (clickedItem == null) return;
		PickedPosition = clickedItem.position;
		item = clickedItem.gameObject;
		state = State.drag;
		offset = PickedPosition - clickPosition;
		//Debug.Log("picked up " + item.name);
	}

	void Drag()
	{
		item.transform.position = GetClickPosition() + offset;
	}

	void Drop()
	{
		DropedPosition = item.transform.position;
		state = State.none;
		item = null;
	}

}