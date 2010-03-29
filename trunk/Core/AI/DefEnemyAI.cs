using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPSGame.Object;
using FPSGame.Engine;
using Microsoft.Xna.Framework;

namespace FPSGame.Core.AI
{
    public class DefEnemyAI : IEnemyAI
    {
        #region IEnemyAI Members

        public const int MAX_SHOOTING_ENEMIES = 5;

        public static int currentShootingNumber = 0;

        private SimpleCharacter character;

        private IEnemyState initialState;

        private IEnemyState currentState;

        private bool began = false;

        public DefEnemyAI(SimpleCharacter character)
        {
            this.character = character;
        }

        public void SetInitialState(IEnemyState state)
        {
            this.initialState = state;
        }

        public void Begin()
        {
            began = true;
            SetState(initialState);
        }

        public void SetState(IEnemyState state)
        {
            if (currentState != null)
            {
                if (state.GetType() == currentState.GetType() && state.CanInterrupted())
                    return;
                currentState.End();
            }
            FPSGame.GetInstance().SetInfo2("State changed from "+currentState+" to "+state);
            currentState = state;
            currentState.Begin();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!began) return;
            if (currentState != null)
                currentState.Update(gameTime);
        }

        public void Notify(global::FPSGame.Engine.GameEvent.IGameEvent evt)
        {
            //System.Windows.Forms.MessageBox.Show(evt.GetSource() + " / " + character);

            if (!began) return;

            if (evt.RequireSource() && evt.GetSource() != character) return;

            if (evt.RequireTarget() && evt.GetTarget() != character) return;

            if (evt.GetEventName() == GameEventGenerator.EVENT_SPOT_PLAYER)
            {
                //player is spotted, try to shoot him!
                ChangeToShootState();
            }

            else if (evt.GetEventName() == GameEventGenerator.EVENT_PLAYER_HIT)
            {
                //player is attacking us, try to shoot him!
                ChangeToShootState();
            }

            else if (evt.GetEventName() == GameEventGenerator.EVENT_PLAYER_SHOOT)
            {
                ExamineSound(evt.GetEventData());
            }

            else if (evt.GetEventName() == GameEventGenerator.EVENT_PLAYER_MOVE)
            {
                ExamineSound(evt.GetEventData());
            }

            else if (evt.GetEventName() == GameEventGenerator.EVENT_PLAYER_NOT_SPOTTED)
            {
                //the player is probably running away!
                if (currentState is ShootState)
                {
                    if (!currentState.IsUpdated()) return;

                    //yeah, the player is obviously running away
                    if (character.IsGuarding())
                    {
                        //if we're guarding, dont' follow him!
                        //SetState(new GuardState(character));
                        ChangeToFindState();
                    }
                    else
                    {
                        //otherwise, follow him
                        ChangeToFindState();
                    }
                }
            }

            else if (evt.GetEventName() == GameEventGenerator.EVENT_RESET_STATE)
            {
                if (initialState is PatrolState)
                    SetState(new PatrolState(character, Vector3.Zero, Vector3.Zero));
                else if (initialState is GuardState)
                    SetState(new GuardState(character));
                else if (initialState is IdleState)
                    SetState(new IdleState(character));
            }
        }

        #endregion

        //Examine the sound generated from player (shooting, walking...)
        //range: the maximum range which this sound can be heard
        private void ExamineSound(String range)
        {
            //first we check to see if we can 'hear' the player
            if (CanHearPlayer(range))
            {
                //rotate this unit to where we 'hear' the sound, i.e. the player's current position
                character.GetCamera().FacePlayer();

                //second we check to see if we can 'see' the player
                if (CanSeePlayer())
                {
                    //if we can 'see' him, shoot him!
                    ChangeToShootState();
                }
                else
                {
                    //if we can't, find him!
                    ChangeToFindState();
                }
            }
        }

        private void ChangeToFindState()
        {
            SetState(new FollowState(character, character.GetPosition(), FPSGame.GetInstance().GetFPSCamera().GetPosition()));
        }

        private Vector3 GetPlayerPosition()
        {
            return FPSGame.GetInstance().GetFPSCamera().GetPosition();
        }

        private bool CanHearPlayer(String range)
        {
            return CanHear(GetPlayerPosition(), range);
        }

        private bool CanHear(Vector3 dst, String range)
        {
            float dist = MathUtils.GetDistance(character.GetPosition(), dst);
            return (dist <= float.Parse(range));
        }

        private bool CanSeePlayer()
        {
            return character.GetCamera().IsPositionVisible(GetPlayerPosition());
        }

        private bool CanSee(Vector3 dst)
        {
            return character.GetCamera().IsPositionVisible(dst);
        }

        private void ChangeToShootState()
        {
            //generate a random number
            //to determine if we shoot change to shootstate
            //int r = MathUtils.Random(0, 100);
            //if (r < 60) return;

            //if current shooting units are large enough, we don't have to shoot player!
            if (currentShootingNumber >= MAX_SHOOTING_ENEMIES)
                return;

            //change the current state to shootstate if we haven't changed already
            SetState(new ShootState(character));
        }
    }
}
