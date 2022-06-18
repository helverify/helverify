#!/bin/sh

ethdir=/home/eth

geth --datadir $ethdir/data --networkid 13337 --port $PORT --syncmode full --nat extip:`dig +short host.docker.internal` --unlock `cat $ethdir/address` --password $ethdir/password --bootnodes= > $ethdir/eth.log 2>&1 &

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

geth attach $ethdir/data/geth.ipc --exec admin.nodeInfo.enode > enode