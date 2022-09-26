#!/bin/sh

ethdir=/home/eth

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

for element in $(jq '.nodes[]' $ethdir/nodes.json);
do
    geth attach --exec "admin.addPeer($element)" $ethdir/data/geth.ipc 
done

geth attach --exec 'miner.start(1)' $ethdir/data/geth.ipc