#!/bin/bash

## Requires Docker CLI installed

## Create a Docker volume
docker volume create --name melkvee_rantsoen_volume
docker volume create --name basal_ration_volume
docker volume create --name postgres_data
docker volume create --name rabbitmqdata

## Rebuild all services with Docker Compose
## If you want to rebuild only a specific service, navigate to the src folder
## and execute `docker compose build <servicename-lowercase>`
docker compose build --force-rm
