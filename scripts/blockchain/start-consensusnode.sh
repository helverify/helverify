#!/bin/sh

ethdir=/home/eth

pkill geth

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --targetgaslimit="100000000" --nat extip:`dig +short host.docker.internal` --unlock `cat $ethdir/address` --password $ethdir/password --bootnodes= > $ethdir/eth.log 2>&1 &
