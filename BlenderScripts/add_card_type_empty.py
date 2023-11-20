import bpy

# Define the details for the empties
empties = [
    {"name": "CardType", "location": (-2.0, 0.0, 3.055)},
    {"name": "SubCardType", "location": (1.42, 0.0, 3.075)},
]

# Function to create an empty and parent it to the main object
def create_and_parent_empty(name, location, parent_name="Card"):
    # Check if the empty already exists
    if name not in bpy.data.objects:
        # Create a new empty
        bpy.ops.object.empty_add(type='PLAIN_AXES', location=location)
        # Get the created empty and rename it
        created_empty = bpy.context.active_object
        created_empty.name = name
        # Parent the empty to the main object
        parent_obj = bpy.data.objects.get(parent_name)
        if parent_obj:
            created_empty.parent = parent_obj
        else:
            print(f"No object named '{parent_name}' found to parent to.")
    else:
        print(f"An object named '{name}' already exists.")

# Check if the main object is named "Cube" and rename it to "Card"
main_object = bpy.data.objects.get("Cube")
if main_object:
    main_object.name = "Card"
    main_object.data.name = "Card"

# Create and parent the empties
for empty in empties:
    create_and_parent_empty(empty["name"], empty["location"])