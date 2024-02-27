#!/bin/bash

# Define the name of the output file based on the user's name or a default
NAME=$(git config user.name | sed s/[^[:alnum:]+._-]//g)
OUTPUT_FILE="submission_${NAME:-code}.zip"

# Add all changes including untracked files, and commit
git add --all
git commit --allow-empty -am "chore(jenga): Prepares submission archive."

# Create a zip archive of the specified directories and root-level files
git archive --format=zip --output="$OUTPUT_FILE" HEAD Assets Packages ProjectSettings

echo "Submission archive created: $OUTPUT_FILE"
