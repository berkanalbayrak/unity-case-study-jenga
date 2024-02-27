# Define the name of the output file based on the user's name or a default
$NAME = git config user.name -replace '[^[:alnum:]+._-]', ''
$OUTPUT_FILE = "submission_${NAME -replace '^\s+|\s+$',''}code.zip"

# Add all changes including untracked files, and commit
git add --all
git commit --allow-empty -am "chore(jenga): Prepares submission archive."

# Create a zip archive of the needed directories
git archive --format=zip --output=$OUTPUT_FILE HEAD Assets Packages ProjectSettings

Write-Host "Submission archive created: $OUTPUT_FILE"
