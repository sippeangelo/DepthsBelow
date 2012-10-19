using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DepthsBelow
{
	/// <summary>
	/// Keeps track of whose turn it is.
	/// </summary>
	public class TurnManager
	{
		/// <summary>
		/// A player turn token.
		/// </summary>
		public class Token
		{
			/// <summary>
			/// Unique identifying name of the token.
			/// </summary>
			public string Name { get; private set; }
			private TurnManager turnManager;


			/// <summary>
			/// Initializes a new instance of the <see cref="Token" /> class.
			/// Preferably use <see cref="TurnManager.CreateToken" /> instead.
			/// </summary>
			/// <param name="turnManager">The turn manager the token belongs to.</param>
			/// <param name="name">Unique identifying name of the token.</param>
			public Token(TurnManager turnManager, string name)
			{
				this.turnManager = turnManager;
				this.Name = name;
				turnManager.AddToken(this);
			}
		}

		/// <summary>
		/// Dictionary of turn tokens.
		/// </summary>
		public Dictionary<string, Token> Tokens;
		private int currentTokenIndex;

		/// <summary>
		/// The turn token belonging to a player.
		/// </summary>
		public Token CurrentTurn
		{
			get { return Tokens.ElementAt(currentTokenIndex).Value; }
		}

		/// <summary>
		/// Get the turn token by the specified name.
		/// </summary>
		/// <param name="name">The identifying name of the player.</param>
		/// <returns>Returns a player turn token.</returns>
		public Token this[string name]
		{
			get { return this.Tokens[name]; }
		}

		/// <summary>
		/// Create a turn manager.
		/// </summary>
		public TurnManager()
		{
			Tokens = new Dictionary<string, Token>();
		}

		/// <summary>
		/// Create a turn manager.
		/// </summary>
		/// <param name="tokenNames">Tokens of players.</param>
		public TurnManager(string[] tokenNames)
			: this()
		{
			foreach (var tokenName in tokenNames)
				CreateToken(tokenName);
		}

		/// <summary>
		/// Create a turn token.
		/// </summary>
		/// <param name="name">Name of the token.</param>
		/// <returns>Returns a player turn token.</returns>
		public Token CreateToken(string name)
		{
			return Tokens.ContainsKey(name) ? Tokens[name] : new Token(this, name);
		}

		/// <summary>
		/// Add a turn token to 
		/// </summary>
		/// <param name="token"></param>
		public void AddToken(Token token)
		{
			if (!Tokens.ContainsKey(token.Name))
				Tokens.Add(token.Name, token);
		}

		/// <summary>
		/// End the current turn, skipping to the next token in the dictionary.
		/// </summary>
		public void EndTurn()
		{
			var prevTurn = CurrentTurn;

			if (++currentTokenIndex > Tokens.Count - 1)
				currentTokenIndex = 0;

			Debug.WriteLine(prevTurn.Name + " turn ended. Current turn: " + CurrentTurn.Name);
		}
	}
}
