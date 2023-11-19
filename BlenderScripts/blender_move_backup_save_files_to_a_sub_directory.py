bl_info = {
    "version": (1, 4),
    "blender": (3, 6, 0),
    "name": "Move Backup Save Files To A Sub Directory",
    "author": "Jonathan Stroem, Updated for Blender 3.6",
    "description": "Move the backup save files to a sub-directory instead of the Blender-file's current directory.",
    "category": "System",
    "location": "File > Preferences > File Paths > Save Versions",
}

# Imports
import bpy
import os
import shutil
from bpy.app.handlers import persistent

# When enabling the addon.
def register():
    bpy.app.handlers.save_post.append(move_handler)

# When disabling the addon.
def unregister():
    bpy.app.handlers.save_post.remove(move_handler)

# So that Blender recognizes it as an addon.
if __name__ == "__main__":
    register()

import bpy
import os
import shutil
from bpy.app.handlers import persistent

@persistent
def move_handler(dummy):
    print("Save handler triggered.")

    # Retrieve the user preferences for save versions
    preferences = bpy.context.preferences
    filepaths = preferences.filepaths
    backup_amount = filepaths.save_version
    print(f"Backup amount: {backup_amount}")

    # Define the backup directory name
    backup_directory = "backup.blend"

    # Full path to the data file
    fullpath = bpy.data.filepath
    if not fullpath:
        print("No file path found. This file has not been saved yet.")
        return

    # Directory the data file is in
    directory = os.path.dirname(fullpath)
    # Name of file, without extension
    name = bpy.path.display_name_from_filepath(fullpath)
    # Extension of files
    extension = ".blend"

    print(f"Full path: {fullpath}")
    print(f"Directory: {directory}")
    print(f"Name: {name}")

    # Create backup directory if it doesn't exist
    backup_directory_path = os.path.join(directory, backup_directory)
    if not os.path.exists(backup_directory_path):
        os.makedirs(backup_directory_path)
        print(f"Backup directory created at {backup_directory_path}")

    # Move the backup files
    for i in range(1, backup_amount + 1):
        backup_file = f"{fullpath}{i}"
        if os.path.isfile(backup_file):
            print(f"Found backup file to move: {backup_file}")

            # Find the next available slot in the backup directory
            for j in range(1, backup_amount + 1):
                new_backup_file = os.path.join(backup_directory_path, f"{name}{extension}{j}")
                if not os.path.exists(new_backup_file):
                    print(f"Moving {backup_file} to {new_backup_file}")
                    shutil.move(backup_file, new_backup_file)
                    break

