using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GameManagment
{
    public  class SkinLoader
    {


       

        readonly static string pathToIdleAnim = "\\1_idle";
      readonly  static string pathToWalkAnim = "\\2_walk";
        readonly static string pathToRunAnim = "\\3_run";
        readonly static string pathToAttackAnim = "\\4_attack";
        readonly static string pathToBlockAnim = "\\5_block";
        readonly static string pathToDieAnim = "\\6_die";


        enum AnimationType
        {
            Idle, Walk, Run, Attack, Block, Die
        }



      static  List<Sprite> LoadSpriteForAnimation(string basePath, AnimationType animationType)
        {
            string animName ="";
            string newPath = "";

            switch (animationType)
            {
                case AnimationType.Idle:
                    basePath += pathToIdleAnim;
                    newPath = basePath.Replace(pathToIdleAnim, "");

                    animName = AnimationType.Idle.ToString();
                    break;

                case AnimationType.Walk:
                    basePath += pathToWalkAnim;
                    newPath = basePath.Replace(pathToWalkAnim, "");

                    animName = AnimationType.Walk.ToString();
                    break;

                case AnimationType.Run:
                    basePath += pathToRunAnim;
                    newPath = basePath.Replace(pathToRunAnim, "");

                    animName = AnimationType.Run.ToString();
                    break;

                case AnimationType.Attack:
                    basePath += pathToAttackAnim;
                    newPath = basePath.Replace(pathToAttackAnim, "");

                    animName = AnimationType.Attack.ToString();
                    break;

                case AnimationType.Block:
                    basePath += pathToBlockAnim;
                    newPath = basePath.Replace(pathToBlockAnim, "");

                    animName = AnimationType.Block.ToString();
                    break;

                case AnimationType.Die:
                    basePath += pathToDieAnim;
                    newPath = basePath.Replace(pathToDieAnim,"");
                    animName = AnimationType.Die.ToString();
                    break;
            }



            Sprite[] spritesArray = Resources.LoadAll<Sprite>(basePath);
            List<Sprite> sprites = new List<Sprite>(spritesArray);

            //CreateAnimations(sprites, newPath,animName,animationType);

            return sprites;
        }


      //static  void CreateAnimations(List<Sprite> sprites, string pathToSaveFolder,string animName,AnimationType animationType)
      //  {
      //      if(animationType == AnimationType.Block)
      //      {
      //          AnimationClip riseAnimationClip = new AnimationClip();
      //          riseAnimationClip.frameRate = 24;
      //          riseAnimationClip.wrapMode = WrapMode.Once;

      //          AnimationClip lowerAnimationClip = new AnimationClip();
      //          lowerAnimationClip.frameRate = 24;
      //          lowerAnimationClip.wrapMode = WrapMode.Once;


      //          EditorCurveBinding fspriteBinding = new EditorCurveBinding();
      //          fspriteBinding.type = typeof(SpriteRenderer);
      //          fspriteBinding.path = "";
      //          fspriteBinding.propertyName = "m_Sprite";

      //          ObjectReferenceKeyframe[] fspriteReferenceKeyframes = new ObjectReferenceKeyframe[7];
      //          int i = 0;
      //          for ( ; i < 7; i++)
      //          {
      //              fspriteReferenceKeyframes[i] = new ObjectReferenceKeyframe();
      //              fspriteReferenceKeyframes[i].time = i / riseAnimationClip.frameRate;
      //              fspriteReferenceKeyframes[i].value = sprites[i];
      //          }


      //          EditorCurveBinding dspriteBinding = new EditorCurveBinding();
      //          dspriteBinding.type = typeof(SpriteRenderer);
      //          dspriteBinding.path = "";
      //          dspriteBinding.propertyName = "m_Sprite";

      //          ObjectReferenceKeyframe[] dspriteReferenceKeyframes = new ObjectReferenceKeyframe[5];

      //          for (int j = 0; j < 5; j++,i++)
      //          {
      //              dspriteReferenceKeyframes[j] = new ObjectReferenceKeyframe();
      //              dspriteReferenceKeyframes[j].time = j / lowerAnimationClip.frameRate;
      //              dspriteReferenceKeyframes[j].value = sprites[i];
      //          }

      //          AnimationUtility.SetObjectReferenceCurve(riseAnimationClip, fspriteBinding, fspriteReferenceKeyframes);
      //          AnimationUtility.SetObjectReferenceCurve(lowerAnimationClip, dspriteBinding, dspriteReferenceKeyframes);


      //          AssetDatabase.CreateAsset(riseAnimationClip, "Assets\\Resources\\" + pathToSaveFolder + "\\" + "RiseShield" + ".anim");
      //          AssetDatabase.CreateAsset(lowerAnimationClip, "Assets\\Resources\\" + pathToSaveFolder + "\\" + "LowerShield" + ".anim");
      //          AssetDatabase.SaveAssets();

      //          return;
      //      }


      //      AnimationClip animationClip = new AnimationClip();
      //      animationClip.frameRate = 24;

      //      if(animationType == AnimationType.Attack || animationType == AnimationType.Die)
      //      {
      //          animationClip.wrapMode = WrapMode.Once;
      //      }

           


      //      EditorCurveBinding spriteBinding = new EditorCurveBinding();
      //      spriteBinding.type = typeof(SpriteRenderer);
      //      spriteBinding.path = "";
      //      spriteBinding.propertyName = "m_Sprite";

      //      ObjectReferenceKeyframe[] spriteReferenceKeyframes = new ObjectReferenceKeyframe[sprites.Count];

      //      for (int i = 0; i < sprites.Count; i++)
      //      {
      //          spriteReferenceKeyframes[i] = new ObjectReferenceKeyframe();
      //          spriteReferenceKeyframes[i].time = i / animationClip.frameRate;
      //          spriteReferenceKeyframes[i].value = sprites[i];
      //      }

      //      AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, spriteReferenceKeyframes);

      //      AssetDatabase.CreateAsset(animationClip, "Assets\\Resources\\" + pathToSaveFolder +"\\" + animName+".anim");
      //      AssetDatabase.SaveAssets();

      //  }











        public static void UploadSkins(List<SpriteHolder> maleSpritesList, List<SpriteHolder> femaleSpritesList)
        {

            SpriteHolder spriteHolderMale = new SpriteHolder();
            SpriteHolder spriteHolderFemale = new SpriteHolder();

            for (int i = 0; i < 30; i++)
            {
                int index = i + 1;
                string pathToMaleSkins = "Players\\Characters\\hero_" + index + "\\male";
                string pathToFemaleSkins = "Players\\Characters\\hero_" + index + "\\female";

                    spriteHolderMale = new SpriteHolder();
                    spriteHolderFemale = new SpriteHolder();
                for (int j = 0; j < 6; j++)
                {

                    spriteHolderMale.sprites.Add(LoadSpriteForAnimation(pathToMaleSkins, (AnimationType)j));

                    spriteHolderFemale.sprites.Add(LoadSpriteForAnimation(pathToFemaleSkins, (AnimationType)j));
                }

                maleSpritesList.Add(spriteHolderMale);
                femaleSpritesList.Add(spriteHolderFemale);

            }
        }

       public static AnimationClip[] GetAnimations(string gender,string index)
        {

              

            string pathToMaleSkins = "Players\\Characters\\hero_" + index + "\\male";
            string pathToFemaleSkins = "Players\\Characters\\hero_" + index + "\\female";

            if (gender == "male")
            {
                return Resources.LoadAll<AnimationClip>(pathToMaleSkins);
               
            }

            if (gender == "female")
            {
                return Resources.LoadAll<AnimationClip>(pathToFemaleSkins);
            }

            return null;
        }
    }
}
