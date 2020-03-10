# NeoFPS_BehaviorDesigner
NeoFPS and Behavior Designer integration assets

## Requirements
This repository was created using Unity 2018.4

It requires the assets [NeoFPS](https://assetstore.unity.com/packages/templates/systems/neofps-150179?aid=1011l58Ft) and [Behavior Designer](https://assetstore.unity.com/packages/tools/visual-scripting/behavior-designer-behavior-trees-for-everyone-15277?aid=1011l58Ft).

## Installation
This integration example is intended to be dropped in to a fresh project along with NeoFPS and Behavior Designer.

1. Import NeoFPS and apply the required Unity settings using the NeoFPS Settings Wizard. You can find more information about this process [here](https://docs.neofps.com/manual/neofps-installation.html).

2. Import the Behavior Designer asset.

3. Clone this repository to a folder inside the project Assets folder such as "NeoFPS_BehaviorDesigner"
	
## Integration

This integration provides a number of Behavior Designer tasks that make it easier to integrate Neo FPS into your project. These can be located in the "Actions/Neo FPS" and "Conditional/Neo FPS" categories.

We've tried to make this as self-documenting as possible using the tooltips and documentation strings used in Behavior Designer itself. If you have questions ask them on the [Neo FPS Discord](https://discord.neofps.com/).

We've also included a simple demo scene, see `NeoFPS_BehaviorDesigner_Demo`