#!/bin/bash

MONGODBRS=mongodb://mongo:27017

while true
do
    sleep 2
    mongo ${MONGODBRS} --eval 'getBuildInfo()' && break
    echo "MongoDB @ mongo:27017 is not ready"
done

FILES="$(ls migration-scripts/*.js | sort)"
for file in ${FILES}
do
	echo "Migrating ${file}"
    mongo ${MONGODBRS}/PAYMENTS_GATEWAY ${file}
done
