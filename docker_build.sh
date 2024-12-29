#!/bin/zsh

# Build the docker image and tag it
docker build -t tylersimeone/projectb/manage-tags:latest .

if [ $? -ne 0 ]; then
  echo "Docker build command failed!"
  exit 1
fi

echo "Docker image built successfully."