﻿using System;
using System.Linq;
using System.Text;

namespace dotfool
{

  public class Human : Player 
  { 
    readonly string SMILE = 
      Encoding.UTF32.GetString(
        new byte[]{0x09, 0xF6, 0x01, 0x00}
      );
    public override Card Move()
    { 
      if (!hand.Any())
        return null;

      Show();
      return Get("move", this.Verify);
    }

    public override Card Response()
    {
      Game.ShowTable();
      Show();
      return Get("response", this.CanBeat);
    }

    private Card Get(string action, Func<Card, Boolean> check)
    {
      Card card;
      do 
      {
        card = Input(action);
      } while (card != null && (!card.IsValid() || !check(card)));
    
      return card;
    }

    private Card Input(string action)
    {
      Console.Write($"Your {action}: ");
      string choice = Console.ReadLine();
      Card dummy = new Card();

      if (choice == "d")
      {
        Game.ShowEngine();
        return dummy;
      }
      else if (choice == "p")
        return null;
      else
      {
        try
          {
            int n = int.Parse(choice);

          if (n < hand.Count)
            return hand[n];
          else
          {
            Console.WriteLine("Use the number of a hand card");
            return dummy;
          }
        }
        catch
        {
          Console.WriteLine($"Invalid input {choice}");
          return dummy;
        }
      } 
    }

    Boolean Verify(Card attack)
    {     
      if (!Game.table.Any())
        return true;
            
      var cards = 
        from card in Game.table
              where card.Rank == attack.Rank
              select card;
      return cards.Any();
    }

    Boolean CanBeat(Card defence)
    {
      Card attack = Game.table.Last();
      return defence.IsGreater(attack);
    }
    
    public override void Show()
    {
      hand = hand.OrderBy(card => card.Rank).ToList();
      Console.Write("Your hand: ");
      for (int i = 0; i < hand.Count; i++)
        Console.Write($"{i}: {hand[i].Code}   ");
      Console.WriteLine();
    }

    public override void Message()
    {
      Console.WriteLine("Engine surrenders, you can add more cards");
      Game.ShowTable();
    }

    public override void Finish()
    {
      Console.WriteLine($"{SMILE} Congrats! You won");
    }
  }
}

