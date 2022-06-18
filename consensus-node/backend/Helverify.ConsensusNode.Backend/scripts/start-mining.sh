#!/bin/sh

ethdir=/home/eth

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

for element in $(jq '.nodes[]' nodes);
do
    geth attach $ethdir/data/geth.ipc --exec "admin.addPeer($element)"
done

geth attach $ethdir/data/geth.ipc --exec 'miner.start(1)'