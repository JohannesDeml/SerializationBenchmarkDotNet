#!/bin/bash

if [ -z "$1" ]
then
  mode="All"
else 
  mode="$1"
fi

./bin/SerializationBenchmarkDotNet-Linux/SerializationBenchmarkDotNet -b "$mode"
