"""
Example demonstrating Proof Verification.

First Issuer creates Claim Definition for existing Schema.
After that, it issues a Claim to Prover (as in issue_credential.py example)

Once Prover has successfully stored its Claim, it uses Proof Request that he
received, to get Claims which satisfy the Proof Request from his wallet.
Prover uses the output to create Proof, using its Master Secret.
After that, Proof is verified against the Proof Request
"""
import sys, os
import asyncio
import json
import pprint
from pathlib import Path

import System

import UnityEngine
import UnityEditor
from UnityEngine.UI import Text


async def unity_test():
    UnityEngine.Debug.Log("start unity_test")

    ## Camera Test
    camera = UnityEngine.Camera.allCameras
    
    for x in camera:
        UnityEngine.Debug.Log(x.name)
        camera1 = x.name
    ##UnityEngine.Debug.Log(camera.GetInstanceID())

    ## Object Test
    game_object = UnityEngine.GameObject.Find("Switch")
    UnityEngine.Debug.Log(game_object.name)

    ## MenuItem Test
    UnityEditor.EditorApplication.ExecuteMenuItem('GameObject/Align View to Selected')
    UnityEngine.Debug.Log("Menu Execute")

    ## Selection Test
    camera_ = UnityEngine.GameObject.Find(camera1)
    UnityEngine.Debug.Log(camera_.name)
    selList = [camera_.GetInstanceID()]
    UnityEngine.Debug.Log(selList)
    selection = System.Array[int](selList)
    UnityEngine.Debug.Log(selection)
    UnityEditor.Selection.instanceIDs = selection
    UnityEngine.Debug.Log("Selection ID")

    ## Text Test
    text_ = UnityEngine.GameObject.Find("TestText")
    UnityEngine.Debug.Log(text_.name)
    text_.SetActive(False)
    UnityEngine.Debug.Log("False!!")
    text_.SetActive(True)
    UnityEngine.Debug.Log("True!!")

    text = text_.GetComponent<Text>()
    UnityEngine.Debug.Log(text.name)
    UnityEngine.Debug.Log(text.text)
    text.text = "python text!!"
    UnityEngine.Debug.Log(text.text)
    UnityEngine.Debug.Log("Change Text")


def main():
    UnityEngine.Debug.Log("START!!!")
    asyncio.set_event_loop_policy(asyncio.WindowsSelectorEventLoopPolicy())
    asyncio.run(unity_test())
    """
    loop = asyncio.get_event_loop()
    loop.run_until_complete(unity_test())
    loop.close()
    """

main()