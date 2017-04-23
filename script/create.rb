#!/usr/bin/env ruby

ORIG_NAME=/PlayGround/i
ORIG_NAME_SED="PlayGround"
NEW_NAME=ARGV[0]
ROOT_DIR="#{File.dirname(__FILE__)}/.."

TO_EXCLUDE = [
  /^ext/,
  /^.git/,
  /^script/,
  /\.png$/,
]

unless NEW_NAME
  puts "Usage: script/create new-name"
  puts ""
  puts "Note: Don't put '.' in the new name and keep it simple (i.e. one word)"
  exit 1
end

stuff_to_find = [
  "find \"#{ROOT_DIR}\" -type d",
  "find \"#{ROOT_DIR}\" -type f",
]

stuff_to_find.each do |cmd|
  to_rename = `#{cmd}`.lines.map {|x| x.chomp}.select {|x| x =~ ORIG_NAME}
  to_rename.each {|x| `git mv "#{x}" "#{x.gsub(ORIG_NAME, NEW_NAME)}" 2>&1`}
end

`git commit -m "Rename Project to #{NEW_NAME}"`

files = `git ls-files`.lines.select{|x| !TO_EXCLUDE.detect{|re| re.match x} }.map {|x| File.realpath "#{ROOT_DIR}/#{x.chomp}"}

files.each {|x| `sed -e 's/#{ORIG_NAME_SED}/#{NEW_NAME}/g' -i '' "#{x}" && git add "#{x}"`}
`git commit --amend -C HEAD`

`git remote set-url origin https://github.com/github/#{NEW_NAME.downcase}`

# :vim tw=120 ts=4 sw=4 et syn=ruby :
